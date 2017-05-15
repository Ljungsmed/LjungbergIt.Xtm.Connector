using LjungbergIt.Xtm.Connector.AddForTranslation;
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

namespace LjungbergIt.Xtm.Connector.Export
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
        foreach (Item translationFolderItem in queuedItemsFolder.GetChildren())
        {
          //if the translation job has an Xtm template, set a string with the id of the Xtm template, otherwise leave it as an empty string
          string xtmTemplateId = string.Empty;
          string xtmTemplateItemId = translationFolderItem[ScConstants.SitecoreFieldIds.QueuFolderXtmTemplate];
          if (xtmTemplateItemId != "")
          {
            //Get the actual ID that corrosponds to the ID of the template in XTM
            xtmTemplateId = masterDb.GetItem(xtmTemplateItemId)[ScConstants.SitecoreFieldIds.XtmTemplateId];
          }

          //Set the properties needed for creating a translation project in Xtm
          TranslationProperties translationProperties = new TranslationProperties();
          translationProperties.SourceLanguage = translationFolderItem[ScConstants.SitecoreFieldIds.QueuFolderSourceLanguage];
          translationProperties.TargetLanguage = translationFolderItem[ScConstants.SitecoreFieldIds.QueuFolderTranslateTo];
          translationProperties.XtmTemplate = xtmTemplateId;
          translationProperties.ItemId = translationFolderItem.ID.ToString();

          //Get an ItemList of all the pages/items for the translation job
          ItemList queuedItemList = buildQueuedItemList(translationFolderItem, translationProperties.SourceLanguage);

          //If the translation job folder is empty do nothing
          if (queuedItemList.Count != 0)
          {
            List<XtmTranslationFile> translationFileList = new List<XtmTranslationFile>();
            int count = 0;
            string firstItemName = string.Empty;
            foreach (Item queuedItem in queuedItemList)
            {
              //string filePath = "~\\" + ScConstants.Misc.translationFolderName + "\\" + ScConstants.Misc.filesFortranslationFolderName + "\\" + translationFolderItem.Name + "_" + queuedItem.Name;
              string completeFilePath = filePath + translationFolderItem.Name + "_" + queuedItem.Name;
              string xmlFilePath = completeFilePath + ".xml";
              string htmlFilePath = completeFilePath + ".html";
              //string xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(filePath + ".xml");
              //string htmlFilePath = System.Web.HttpContext.Current.Server.MapPath(filePath + ".html");

              XtmTranslationFile translationFile = new XtmTranslationFile();
              translationFile.FilePath = xmlFilePath;
              string ItemId = queuedItem[ScConstants.XtmTranslationQueueItemTemplate.ItemId];
              Item translationItem = masterDb.GetItem(ItemId);
              translationFile.FileName = translationItem.Name;
              if (count == 0)
              {
                firstItemName = translationItem.Name;
              }
              //TODO move to Files.cs
              //Create an XML file per page/item in the translation job
              using (FileStream fs = new FileStream(xmlFilePath, FileMode.Create))
              {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                XmlWriter xw = XmlWriter.Create(fs, settings);
                UpdateItem updateItem = new UpdateItem();
                List<UpdateItem> updateList = new List<UpdateItem>();
                updateList.Add(new UpdateItem() { FieldIdOrName = ScConstants.SitecoreXtmTemplateFieldIDs.InTranslation, FieldValue = "1" });

                xw.WriteStartElement(ScConstants.XmlNodes.XmlRoot);

                Item itemInTranslation = masterDb.GetItem(queuedItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_ItemId]);
                AddElement(xw, queuedItem, translationProperties.SourceLanguage);
                updateItem.Update(masterDb.GetItem(itemInTranslation.ID), updateList);

                xw.WriteEndElement();
                xw.Flush();
                xw.Close();
              }

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
            projectName.Append(firstItemName);
            if (count != 1)
            {
              projectName.Append(" + ");
              projectName.Append((count - 1).ToString());
              if (count == 2)
              {
                projectName.Append(" page");
              }
              else
              {
                projectName.Append(" pages");
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
              //TODO use below instead when CreateItem is changed to return the item
              //UpdateItem updateItem = new UpdateItem();
              //updateItem.CreateItem(DateTime.Now.ToString("yyyyMMdd-HHmmss"), masterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueArchiveFolder), ScConstants.SitecoreTemplates.TranslationQueueLanguageFolderTemplate, )
              using (new SecurityDisabler())
              {
                Item archiveFolder = masterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueArchiveFolder);
                string itemName = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                Item newArchive = archiveFolder.Add(itemName, ScConstants.SitecoreTemplates.TranslationQueueLanguageFolderTemplate);
                translationFolderItem.MoveTo(newArchive);
              }
            }
          }
          else
          {
            StringBuilder message = new StringBuilder();
            message.Append("The translation job folder ");
            message.Append(translationFolderItem.Name);
            message.Append(" did not contain any pages/items");
            returnMessageList.Add(new ReturnMessage { Success = false, Message = message.ToString(), Item = translationFolderItem });
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

    private void ArchiveItem(Item itemToArchive)
    {
      Item archiveFolder = masterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueArchiveFolder);
      using (new SecurityDisabler())
      {
        itemToArchive.MoveTo(archiveFolder);
      }
    }

    private XmlWriter AddElement(XmlWriter xw, Item ItemToAdd, string language)
    {

      //TODO add a try catch 
      Language SourceLanguage = Language.Parse(language);
      //Sitecore.Data.Version version = Sitecore.Data.Version.Parse(ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_Version]);
      ID ItemId = new ID(ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_ItemId]);

      Item ItemToTranslate = ScConstants.SitecoreDatabases.MasterDb.GetItem(ItemId, SourceLanguage);

      xw.WriteStartElement(ScConstants.XmlNodes.XmlRootElement);
      xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeId, ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_ItemId]);
      xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeLanguage, ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_TranslateTo]);
      xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeVersion, ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_Version]);
      xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeSourceLanguage, language);
      xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeAddedBy, ItemToAdd[ScConstants.SitecoreFieldIds.TranslationQueueItem_AddedBy]);

      //one for each field

      foreach (Field field in ItemToTranslate.Fields)
      {
        if (!field.Name.Contains("__") && !field.Name.Contains("XTM_"))
        {
          //if the field is shared, it should not be translated
          if (!field.Shared)
          {
            //private static Regex _invalidXMLChars = new Regex(
            //@"(?<![\uD800-\uDBFF])[\uDC00-\uDFFF]|[\uD800-\uDBFF](?![\uDC00-\uDFFF])|[\x00-\x08\x0B\x0C\x0E-\x1F\x7F-\x9F\uFEFF\uFFFE\uFFFF]",
            //RegexOptions.Compiled);
            //XmlConvert.IsXmlChar
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
              exportInfo.UpdateValueField(error,ItemToTranslate.ID.ToString(), field.Name, false);
              //string statusFieldValue = exportInfo.StatusField.Value;
            }
            
            if (fieldValueOk)
            {
              xw.WriteStartElement(ScConstants.XmlNodes.XmlElementField);
              xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeFieldName, field.Name);
              xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeFieldType, field.Type);
              xw.WriteString(field.Value);
              xw.WriteEndElement();
            }            
          }
        }
      }

      xw.WriteEndElement();
      return xw;
    }
  }
}
