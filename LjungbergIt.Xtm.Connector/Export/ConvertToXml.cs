﻿using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Connector.Helpers;

using LjungbergIt.Xtm.Webservice;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace LjungbergIt.Xtm.Connector.Helpers
{
  public class ConvertToXml
  {
    Database masterDb = ScConstants.SitecoreDatabases.MasterDb;
    public List<ReturnMessage> Transform(string filePath)
    {
      List<ReturnMessage> returnMessageList = new List<ReturnMessage>();

      //get the folder that contains all awaiting translation jobs
      Item queuedItemsFolder = masterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueFolder);

      //Translation job = a number of pages/items added for translation with the same source and target language and Xtm template
      //If there are any translation jobs waiting
      if (queuedItemsFolder.Children.Count != 0)
      {
        //For each translation job 
        foreach (Item translationProjectItem in queuedItemsFolder.GetChildren())
        {
          //if the translation job has an Xtm template, set a string with the id of the Xtm template, otherwise leave it as an empty string
          string xtmTemplateId = string.Empty;
          string xtmTemplateItemId = translationProjectItem[ScConstants.XtmQueueProjectTemplateFolder.XtmTemplate];
          if (xtmTemplateItemId != "")
          {
            //Get the actual ID that corrosponds to the ID of the template in XTM
            xtmTemplateId = masterDb.GetItem(xtmTemplateItemId)[ScConstants.SitecoreFieldIds.XtmTemplateId];
          }

          //Set the properties needed for creating a translation project in Xtm
          TranslationProperties translationProperties = new TranslationProperties();

          //Get the source language
          translationProperties.SourceLanguage = translationProjectItem[ScConstants.XtmQueueProjectTemplateFolder.SourceLanguage];

          //Get target languages and convert to XTM languages
          MultilistField targetLanguagesField = translationProjectItem.Fields[ScConstants.XtmQueueProjectTemplateFolder.TargetLanguages];
          Item[] targetLanguagesItems = targetLanguagesField.GetItems();

          Mapping mapping = new Mapping();

          translationProperties.TargetLanguages = mapping.SitecoreLanguageToXtmLanguage(targetLanguagesItems);
          translationProperties.XtmTemplate = xtmTemplateId;
          translationProperties.ItemId = translationProjectItem.ID.ToString();

          //Set the due date if any
          translationProperties.DueDate = translationProjectItem[ScConstants.XtmQueueProjectTemplateFolder.DueDate];

          //Get an ItemList of all the pages/items for the translation job
          ItemList queuedItemList = buildQueuedItemList(translationProjectItem, translationProperties.SourceLanguage);

          //If the translation job folder is empty do nothing
          if (queuedItemList.Count != 0)
          {
            List<XtmTranslationFile> translationFileList = new List<XtmTranslationFile>();
            //count is used to add the number of pages/items that is in a translation project
            int count = 0;
            //TODO remove as the projects are now created with custom name?
            string firstItemName = string.Empty;

            //Create two lists of translationItems, one with all items not added as related items and one for all the items added as related items
            ItemList mainTranslationItemsList = new ItemList();
            ItemList relatedTranslationItemsList = new ItemList();
            foreach (Item queuedItem in queuedItemList)
            {
              //If the item is a related item (the field "RelatedItemId" have a value) the item is added to a list and prossesed later
              string relatedItemId = queuedItem[TranslationQueueItem.XtmTranslationQueueItem.Field_RelatedItemId];
              if (relatedItemId != "")
              {
                relatedTranslationItemsList.Add(queuedItem);
              }
              else
              {
                mainTranslationItemsList.Add(queuedItem);
              }
            }

            //Create empty list for combined translationItems
            List<TranslationItemCollection> translationItemCollection = new List<TranslationItemCollection>();

            //For each main translation item, add the related translation items
            foreach (Item mainTranslationItem in mainTranslationItemsList)
            {
              translationItemCollection.Add(new TranslationItemCollection(mainTranslationItem, relatedTranslationItemsList));
            }

            //Iterate the the combined translation items list and create XML and HTML files where applicable
            foreach (TranslationItemCollection queuedItemCollection in translationItemCollection)
            {
              //string filePath = "~\\" + ScConstants.Misc.translationFolderName + "\\" + ScConstants.Misc.filesFortranslationFolderName + "\\" + translationFolderItem.Name + "_" + queuedItem.Name;
              string completeFilePath = filePath + translationProjectItem.Name + "_" + queuedItemCollection.MainTranslationItem.Name;
              string xmlFilePath = completeFilePath + ".xml";
              string htmlFilePath = completeFilePath + ".html";
              //string xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(filePath + ".xml");
              //string htmlFilePath = System.Web.HttpContext.Current.Server.MapPath(filePath + ".html");

              XtmTranslationFile translationFile = new XtmTranslationFile();
              translationFile.FilePath = xmlFilePath;
              string ItemId = queuedItemCollection.MainTranslationItem[TranslationQueueItem.XtmTranslationQueueItem.Field_ItemId];
              //Find the actual item that needs to be translated from the ItemId field of the translationQueueItem
              Item translationItem = masterDb.GetItem(ItemId);
              translationFile.FileName = translationItem.Name;

              //TODO Remove as project name is now custom?
              if (count == 0)
              {
                firstItemName = translationItem.Name;
              }

              XmlWriterSettings settings = new XmlWriterSettings();
              settings.Indent = true;
              settings.CloseOutput = true;

              using (XmlWriter xw = XmlWriter.Create(xmlFilePath, settings))
              {
                List<UpdateItem> updateList = new List<UpdateItem>();
                updateList.Add(new UpdateItem() { FieldIdOrName = ScConstants.SitecoreXtmTemplateFieldIDs.InTranslation, FieldValue = "1" });
                UpdateItem updateItem = new UpdateItem();
                updateItem.Update(masterDb.GetItem(translationItem.ID), updateList);

                //Add the root element "XtmSitecoreTranslation", to the xml file
                xw.WriteStartElement(ScConstants.XmlNodes.XmlRoot);

                AddElement(xw, queuedItemCollection.MainTranslationItem, translationProperties.SourceLanguage);

                //If there are any related items in the collection, add those to the XML file
                if (queuedItemCollection.RelatedItemsList != null)
                {
                  foreach (Item relatedItem in queuedItemCollection.RelatedItemsList)
                  {
                    AddElement(xw, relatedItem, translationProperties.SourceLanguage);
                    Item item = masterDb.GetItem(relatedItem[TranslationQueueItem.XtmTranslationQueueItem.Field_ItemId]);
                    if (item != null)
                    {
                      updateItem.Update(item, updateList);
                    }

                  }
                }

                xw.WriteEndElement();
                xw.WriteEndDocument();
                //xw.Flush();                
              }
              //}


              Html html = new Html();
              ReturnMessage htmlCreationReturnMessage = html.GenerateHtml(translationItem, htmlFilePath);

              if (htmlCreationReturnMessage.Success)
              {
                translationFile.HtmlFilePath = htmlFilePath;
                translationFile.HtmlFileAvailable = true;
              }
              else
              {
                translationFile.HtmlFileAvailable = false;
                //TODO should there be a warning when no preview/HTML could be created??
                //returnMessageList.Add(htmlCreationReturnMessage);
              }

              translationFileList.Add(translationFile);
              count++;

            }

            //Set the project name according to number of pages/items in the translation job
            StringBuilder projectName = new StringBuilder(masterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsItem)[ScConstants.XtmSettingsTemplate.ProjectNamePrefix]);
            projectName.Append(" ");
            projectName.Append(translationProjectItem[ScConstants.XtmQueueProjectTemplateFolder.XTMProjectName]);
            if (count != 0)
            {
              projectName.Append(" (with a total of ");
              projectName.Append((count).ToString());
              if (count == 1)
              {
                projectName.Append(" page)");
              }
              else
              {
                projectName.Append(" pages)");
              }
            }

            //Use XTM web service to create project with all generated XML and HTML files
            StartTranslation startTranslation = new StartTranslation();
            ReturnMessage returnMessage = startTranslation.SendFilesToXtm(translationFileList, projectName.ToString(), translationProperties);
            returnMessageList.Add(returnMessage);
            //DEBUG
            //returnMessage.Success = false;

            if (returnMessage.Success)
            {
              //  //TODO use below instead when CreateItem is changed to return the item
              UpdateItem updateItem = new UpdateItem();
              updateItem.DeleteItem(translationProjectItem);
              //  //updateItem.CreateItem(DateTime.Now.ToString("yyyyMMdd-HHmmss"), masterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueArchiveFolder), ScConstants.SitecoreTemplates.TranslationQueueLanguageFolderTemplate, )
              //  using (new SecurityDisabler())
              //  {
              //    Item archiveFolder = masterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueArchiveFolder);
              //    string itemName = DateTime.Now.ToString("yyyyMMdd-HHmmss");
              //    Item newArchive = archiveFolder.Add(itemName, ScConstants.SitecoreTemplates.TranslationQueueLanguageFolderTemplate);
              //    translationProjectItem.MoveTo(newArchive);
              //  }
            }
          }
          else
          {
            StringBuilder message = new StringBuilder();
            message.Append("The translation job folder ");
            message.Append(translationProjectItem.Name);
            message.Append(" did not contain any pages/items");
            returnMessageList.Add(new ReturnMessage { Success = false, Message = message.ToString(), Item = translationProjectItem });
          }
        }
      }
      else
      {
        returnMessageList.Add(new ReturnMessage { Message = "There are no translation jobs waiting" });
      }

      return returnMessageList;
    }

    private ItemList buildQueuedItemList(Item langFolderItem, string sourceLanguage)
    {
      ItemList queuedItemList = new ItemList();
      Language sitecoreSourceLanguage = Language.Parse(sourceLanguage);
      foreach (Item queuedItem in langFolderItem.GetChildren())
      {
        Item correctLanguageItem = masterDb.GetItem(queuedItem.ID, sitecoreSourceLanguage);
        queuedItemList.Add(correctLanguageItem);
        //ArchiveItem(langFolderItem);
      }

      return queuedItemList;
    }

    //private void ArchiveItem(Item itemToArchive)
    //{
    //  Item archiveFolder = masterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueArchiveFolder);
    //  using (new SecurityDisabler())
    //  {
    //    itemToArchive.MoveTo(archiveFolder);
    //  }
    //}

    private XmlWriter AddElement(XmlWriter xw, Item ItemToAdd, string language)
    {

      //TODO add a try catch 
      Language SourceLanguage = Language.Parse(language);
      //Sitecore.Data.Version version = Sitecore.Data.Version.Parse(ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_Version]);
      ID ItemId = new ID(ItemToAdd[TranslationQueueItem.XtmTranslationQueueItem.Field_ItemId]);

      Item ItemToTranslate = ScConstants.SitecoreDatabases.MasterDb.GetItem(ItemId, SourceLanguage);

      xw.WriteStartElement(ScConstants.XmlNodes.XmlRootElement);
      xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeId, ItemToAdd[TranslationQueueItem.XtmTranslationQueueItem.Field_ItemId]);
      //TODO change all constants to use XtmTranslationQueueItemTemplate
      xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeLanguage, ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_TranslateTo]);
      xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeVersion, ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_Version]);
      xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeSourceLanguage, language);
      xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeAddedBy, ItemToAdd[TranslationQueueItem.XtmTranslationQueueItem.Field_AddedBy]);

      //Get list of field IDs that should be excluded from the Settings item
      XtmSettingsItem xtmSettingsItem = new XtmSettingsItem();
      //one for each field


      foreach (Field field in ItemToTranslate.Fields)
      {
        if (!field.Name.Contains("__") && !field.Name.Contains("XTM_"))
        {
          //if the field is shared, it should not be translated
          if (!field.Shared)
          {
            //Check if field should be excluded based on the Excluded Fields on the Settings item
            if (!xtmSettingsItem.ExcludedFieldsIds.Contains(field.ID.ToString()))
            {
              //Check if the field type should be excluded based on the Excluded Field Types on the settings item
              List<string> excludedFieldsTypes = xtmSettingsItem.GetExcludedFieldTypes();
              bool excludeFieldType = false;
              if (excludedFieldsTypes != null)
              {
                excludeFieldType = excludedFieldsTypes.Contains(field.TypeKey);
              }

              if (!excludeFieldType)
              {
                //Check if field is a date field and exclude field if it is
                bool isDateField = FieldTypeManager.GetField(field) is DateField;
                if (!isDateField)
                {
                  string fieldValue = string.Empty;
                  bool fieldValueOk = true;
                  try
                  {
                    fieldValue = XmlConvert.VerifyXmlChars(field.Value);
                  }
                  catch (XmlException e)
                  {
                    fieldValueOk = false;
                    string error = e.Message;
                    ExportInfo exportInfo = new ExportInfo();
                    exportInfo.UpdateValueField(error, ItemToTranslate.ID.ToString(), field.Name, false);
                    //string statusFieldValue = exportInfo.StatusField.Value;
                  }

                  if (fieldValueOk)
                  {
                    bool isImageField = FieldTypeManager.GetField(field) is ImageField;
                    bool isDefaultAltText = false;
                    string altText = string.Empty;

                    if (isImageField)
                    {
                      ImageField imageField = new ImageField(field);
                      Item mediaItem = imageField.MediaItem;
                      altText = imageField.Alt;
                      string defaultAltText = mediaItem["alt"];
                      if (altText.Equals(defaultAltText))
                      {
                        isDefaultAltText = true;
                      }
                    }

                    if (!isImageField || altText != string.Empty)
                    {
                      xw.WriteStartElement(ScConstants.XmlNodes.XmlElementField);
                      xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeFieldName, field.Name);
                      xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeFieldType, field.Type);

                      //If the field is an image, an extra attribute is added to the XML indicating if the alt text is from the default value of the image item or the image instance itself
                      if (isImageField)
                      {
                        if (isDefaultAltText)
                        {
                          xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeDefaultValue, bool.TrueString);
                        }
                        //xw.WriteString(altText);
                      }

                      xw.WriteString(field.Value);
                      xw.WriteEndElement();
                    }
                  }
                }
              }
            }
            else
            {
              ScLogging scLogging = new ScLogging();
              scLogging.WriteInfo("Field with ID: " + field.ID.ToString() + " was excluded as it is set to be excluded in the XTM settings");
            }
          }
        }
      }

      xw.WriteEndElement();
      return xw;
    }
  }
}
