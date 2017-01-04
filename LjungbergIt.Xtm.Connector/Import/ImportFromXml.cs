﻿using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Webservice;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace LjungbergIt.Xtm.Connector.Import
{
  class ImportFromXml
  {
    Database masterDb = ScConstants.SitecoreDatabases.MasterDb;

    public void CreateTranslatedContentFromProgressFolder()
    {
      Item inProgressFolder = masterDb.GetItem(ScConstants.SitecoreIDs.TranslationInProgressFolder);
      foreach (Item progressItem in inProgressFolder.GetChildren())
      {
        long projectId;
        bool projectIdOk = long.TryParse(progressItem[ScConstants.SitecoreFieldIds.XtmProjectId], out projectId);
        if (projectIdOk)
        {
          bool success = CreateTranslatedContent(projectId, progressItem);
        }
      }
    }

    public bool CreateTranslatedContent(long projectId, Item progressItem)
    {
      XtmProject xtmProject = new XtmProject();
      LoginProperties login = new LoginProperties();
      string client = login.ScClient;
      long userId = login.ScUserId;
      string password = login.ScPassword;
      bool success = new bool();

      XtmWebserviceProperties xtmWebserviceProperties = new XtmWebserviceProperties();

      bool finished = xtmProject.IsTranslationFinished(projectId, client, userId, password, xtmWebserviceProperties.WebserviceEndpoint, xtmWebserviceProperties.IsHttps);
      if (finished)
      {
        XtmHandleTranslatedContent Xtm = new XtmHandleTranslatedContent();
        List<byte[]> bytesList = Xtm.GetFileInBytes(projectId, client, userId, password, xtmWebserviceProperties.WebserviceEndpoint, xtmWebserviceProperties.IsHttps, login.ScIntegrationKey);

        if (bytesList.Count != 0)
        {
          List<XmlDocument> translatedXmlDocs = ConvertTranslatedBytesToXML(projectId, bytesList);
          foreach(XmlDocument xmlDoc in translatedXmlDocs)
          {
            bool result = ReadFromXml(xmlDoc);
            if (result)
            {
              using (new SecurityDisabler())
              {
                //progressItem.Delete();                               
              }
            }
          }         
          success = true;
        }
        else
        {
          success = false;
        }
      }
      return success;
    }

    public List<XmlDocument> ConvertTranslatedBytesToXML(long projectId, List<byte[]> bytesList)
    {
      List<XmlDocument> xmlDocuments = new List<XmlDocument>();
      string filePath = System.Web.HttpContext.Current.Server.MapPath("~\\" + ScConstants.Misc.translationFolderName + "\\" + ScConstants.Misc.filesForImportFolderName + "\\");
      int count = 1;

      foreach (byte[] bytes in bytesList)
      {
        string zipFileName = filePath + "file" + count.ToString() + ".zip";
        string xmlFileName = filePath + "file" + count.ToString() + ".xml";        
        
        File.WriteAllBytes(zipFileName, bytes);
        using (ZipArchive archive = ZipFile.OpenRead(zipFileName))
        {
          IReadOnlyCollection<ZipArchiveEntry> zipEntries = archive.Entries;
          foreach (ZipArchiveEntry entry in zipEntries)
          {
            XmlDocument xmlDoc = new XmlDocument();
            entry.ExtractToFile(xmlFileName, true);
            xmlDoc.Load(xmlFileName);
            xmlDocuments.Add(xmlDoc);
          }
          File.Delete(zipFileName);
          File.Delete(xmlFileName);
        }
        count++;        
      }      
      return xmlDocuments;
    }

    public bool ReadFromXml(XmlDocument xmlDocument)
    {
      try
      {
        XmlNode rootElement = xmlDocument.SelectSingleNode(ScConstants.XmlNodes.XmlRoot);
        foreach (XmlNode node in rootElement)
        {
          if (node.Name.Equals(ScConstants.XmlNodes.XmlRootElement))
          {
            Item translatedItem = CreateNewTranslatedVersion(node);
          }

          if (node.Name.Equals(ScConstants.XmlNodes.XmlAttributeFieldName))
          {

          }
        }

        return true;
      }
      catch (Exception ex)
      {
        ScLogging scLogging = new ScLogging();
        scLogging.WriteError(ex.Message);
        return false;
      }
    }

    private Item CreateNewTranslatedVersion(XmlNode node)
    {
      string sitecoreItemId = node.Attributes[ScConstants.XmlNodes.XmlAttributeId].Value;
      string language = node.Attributes[ScConstants.XmlNodes.XmlAttributeLanguage].Value;
      string sourceLanguage = node.Attributes[ScConstants.XmlNodes.XmlAttributeSourceLanguage].Value;
      string addedBy = node.Attributes[ScConstants.XmlNodes.XmlAttributeAddedBy].Value;

      language = TransformLangauge(language);

      language = language.Replace("_", "-");

      Language SitecoreLanguage = Language.Parse(language);

      Item translationItem = ScConstants.SitecoreDatabases.MasterDb.GetItem(sitecoreItemId, SitecoreLanguage);
      Item newVersion;

      using (new SecurityDisabler())
      {
        newVersion = translationItem.Versions.AddVersion();
      }

      List<UpdateItem> fieldObjects = new List<UpdateItem>();

      string dateNow = DateTime.Now.ToString("yyyyMMddTHHmmssZ");

      fieldObjects.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreStandardFieldNames.Workflow, FieldValue = ScConstants.SitecoreWorkflowIDs.XtmWorkflow });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreStandardFieldNames.WorkflowState, FieldValue = ScConstants.SitecoreWorkflowIDs.XtmWorkflowStateAwaiting });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreXtmTemplateFieldIDs.Translated, FieldValue = "1" });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreXtmTemplateFieldIDs.TranslatedDate, FieldValue = DateTime.Now.ToString("yyyyMMddTHHmmss") });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreXtmTemplateFieldIDs.TranslatedFrom, FieldValue = sourceLanguage });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreXtmTemplateFieldIDs.InTranslation, FieldValue = "0" });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreXtmTemplateFieldIDs.AddedToTranslateionBy, FieldValue = addedBy });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreStandardFieldNames.CreatedBy, FieldValue = "XTM" });

      foreach (XmlNode fieldNode in node)
      {
        UpdateItem field = new UpdateItem();
        field.FieldIdOrName = fieldNode.Attributes[ScConstants.XmlNodes.XmlAttributeFieldName].Value;
        field.FieldValue = fieldNode.InnerText;
        fieldObjects.Add(field);
      }

      UpdateItem updateItem = new UpdateItem();
      updateItem.Update(newVersion, fieldObjects);

      return (translationItem);
    }

    private string TransformLangauge(string languageToTransform)
    {
      Item languageMappingFolder = masterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsLanguageMappingFolder);

      foreach (Item langMapping in languageMappingFolder.GetChildren())
      {
        string xtmLang = ScConstants.SitecoreDatabases.MasterDb.GetItem(langMapping[ScConstants.SitecoreFieldIds.XtmLanguageName]).Name;
        if (xtmLang.Equals(languageToTransform))
        {
          languageToTransform = langMapping[ScConstants.SitecoreFieldIds.SitecoreLanguageName];
        }
      }
      return languageToTransform;
    }
  }
}
