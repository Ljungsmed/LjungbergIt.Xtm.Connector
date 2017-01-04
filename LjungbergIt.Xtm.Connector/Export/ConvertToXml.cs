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
using System.Xml;

namespace LjungbergIt.Xtm.Connector.Export
{
  public class ConvertToXml
  {
    Database masterDb = ScConstants.SitecoreDatabases.MasterDb;

    public string Transform()
    {
      string returnString = "All translation jobs send for translation to XTM";

      Item queuedItemsFolder = masterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueFolder);
      StartTranslation startTranslation = new StartTranslation();

      if (queuedItemsFolder.Children.Count != 0)
      {
        foreach (Item translationFolderItem in queuedItemsFolder.GetChildren())
        {
          string fileName = translationFolderItem.Name;
          string projectName = masterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsItem)[ScConstants.XtmSettingsTemplate.ProjectNamePrefix];
          string xtmTemplateId = string.Empty;
          string xtmTemplateItemId = translationFolderItem[ScConstants.SitecoreFieldIds.QueuFolderXtmTemplate];
          if (xtmTemplateItemId != "")
          {
            xtmTemplateId = masterDb.GetItem(xtmTemplateItemId)[ScConstants.SitecoreFieldIds.XtmTemplateId];
          }

          TranslationProperties translationProperties = new TranslationProperties();
          translationProperties.SourceLanguage = translationFolderItem[ScConstants.SitecoreFieldIds.QueuFolderSourceLanguage];
          translationProperties.TargetLanguage = translationFolderItem[ScConstants.SitecoreFieldIds.QueuFolderTranslateTo];
          translationProperties.XtmTemplate = xtmTemplateId;
          translationProperties.ItemId = translationFolderItem.ID.ToString();

          ItemList queuedItemList = buildQueuedItemList(translationFolderItem, translationProperties.SourceLanguage);

          if (queuedItemList.Count != 0)
          {
            List<XtmTranslationFile> translationFileList = new List<XtmTranslationFile>();
            foreach (Item queuedItem in queuedItemList)
            {
              string filePath = "~\\" + ScConstants.Misc.translationFolderName + "\\" + ScConstants.Misc.filesFortranslationFolderName + "\\" + translationFolderItem.Name + "_" + queuedItem.Name + ".xml";
              string fullFilePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
              XtmTranslationFile translationFile = new XtmTranslationFile();
              translationFile.FilePath = fullFilePath;
              string ItemId = queuedItem[ScConstants.XtmTranslationQueueItemTemplate.ItemId];
              translationFile.FileName = masterDb.GetItem(ItemId).Name;

              //TODO move to Files.cs
              //Add content of all items within a translation queue folder to a single XML file for adding a single project in XTM
              using (FileStream fs = new FileStream(fullFilePath, FileMode.Create))
              {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                XmlWriter xw = XmlWriter.Create(fs, settings);
                UpdateItem updateItem = new UpdateItem();
                List<UpdateItem> updateList = new List<UpdateItem>();
                updateList.Add(new UpdateItem() { FieldIdOrName = ScConstants.SitecoreXtmTemplateFieldIDs.InTranslation, FieldValue = "1" });
                //xw.WriteStartDocument();
                xw.WriteStartElement(ScConstants.XmlNodes.XmlRoot);
                                
                Item itemInTranslation = masterDb.GetItem(queuedItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_ItemId]);
                AddElement(xw, queuedItem, translationProperties.SourceLanguage);
                updateItem.Update(masterDb.GetItem(itemInTranslation.ID), updateList);

                xw.WriteEndElement();
                xw.Flush();
                xw.Close();
              }
              translationFileList.Add(translationFile);
            }

            //Use XTM web service to create project with all generated XML files
            returnString = startTranslation.SendFilesToXtm(translationFileList, projectName, translationProperties);
            if (returnString.Equals("success"))
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
        }
      }
      else
      {
        returnString = "There are no translation jobs waiting";
      }

      return returnString;
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
      Language SourceLanguage = Language.Parse(language);
      //Sitecore.Data.Version version = Sitecore.Data.Version.Parse(ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_Version]);
      Sitecore.Data.ID ItemId = new Sitecore.Data.ID(ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_ItemId]);

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
          xw.WriteStartElement(ScConstants.XmlNodes.XmlElementField);
          xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeFieldName, field.Name);
          xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeFieldType, field.Type);
          xw.WriteString(field.Value);
          xw.WriteEndElement();
        }
      }

      xw.WriteEndElement();
      return xw;
    }
  }
}
