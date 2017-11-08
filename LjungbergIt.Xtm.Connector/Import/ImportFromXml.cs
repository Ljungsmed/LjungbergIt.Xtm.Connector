using LjungbergIt.Xtm.Connector.Helpers;

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
    ScLogging scLogging = new ScLogging();

    //CreateTranslatedContentFromProgressFolder is only used for the scheduled task
    public ReturnMessage CreateTranslatedContentFromProgressFolder()
    {
      ReturnMessage returnMessage = new ReturnMessage();
      Item inProgressFolder = masterDb.GetItem(ScConstants.SitecoreIDs.TranslationInProgressFolder);
      foreach (Item progressItem in inProgressFolder.GetChildren())
      {
        string progressItemId = progressItem[ScConstants.SitecoreFieldIds.XtmProjectId];
        long projectId;
        bool projectIdOk = long.TryParse(progressItemId, out projectId);
        if (projectIdOk)
        {
          bool result = CreateTranslatedContent(projectId, progressItem);
          returnMessage.Success = result;
        }
        else
        {
          returnMessage.Success = false;
          returnMessage.Message = "ProjectId: " + progressItemId + " is not in a correct fortmat";
        }
      }
      return returnMessage;
    }

    //CreateTranslatedContent is directly called by the import button manaully or by the XtmCallback.aspx
    public bool CreateTranslatedContent(long projectId, Item progressItem)
    {
      XtmProject xtmProject = new XtmProject(); 
      LoginProperties login = new LoginProperties();
      string client = login.ScClient;
      long userId = login.ScUserId;
      string password = login.ScPassword;
      XtmProject xtmLoginProject = new XtmProject { Client = client, UserId = userId, Password = password };
      bool success = new bool();

      XtmWebserviceProperties xtmWebserviceProperties = new XtmWebserviceProperties();

      //bool finished = xtmProject.IsTranslationFinished(projectId, client, userId, password, xtmWebserviceProperties.WebserviceEndpoint, xtmWebserviceProperties.IsHttps);
      xtmProject = xtmProject.GetXtmProject(projectId, xtmLoginProject, xtmWebserviceProperties.WebserviceEndpoint, xtmWebserviceProperties.IsHttps);

      if (xtmProject.WorkflowStatus.Equals("FINISHED"))
      {
        XtmHandleTranslatedContent Xtm = new XtmHandleTranslatedContent();
        List<XtmJob> xtmJobList = Xtm.GetTranslatedJobs(projectId, client, userId, password, xtmWebserviceProperties.WebserviceEndpoint, xtmWebserviceProperties.IsHttps);
        
        try
        {
          if (xtmJobList.Count != 0)
          {
            List<ImportProps> translatedXmlDocs = ConvertTranslatedBytesToXML(xtmJobList);
            bool totalSuccess = true;
            foreach (ImportProps xmlDoc in translatedXmlDocs)
            {
              bool result = ReadFromXml(xmlDoc.TranslatedXmlDocument, xmlDoc.TargetLanguage, xtmProject.SourceLanguage, projectId.ToString());
              if (result)
              {
                
              }
              else
              {
                totalSuccess = false;
              }
            }
            success = true;
            if (totalSuccess)
            {
              if (progressItem != null)
              {
                UpdateItem updateItem = new UpdateItem();
                updateItem.DeleteItem(progressItem);
              }
            }
            else
            {
              //TODO show which translated jobs that did not get imported
            }
          }
          else
          {
            success = false;
          }
        }
        catch (Exception e)
        {
          success = false;
          scLogging.WriteError("Something is wrong witht the returned byte array for project with id " + projectId.ToString() + ". following error occured: " + e.Message);
        }
        
      }
      return success;
    }

    public List<ImportProps> ConvertTranslatedBytesToXML(List<XtmJob> xtmJobList)
    {
      List<XmlDocument> xmlDocuments = new List<XmlDocument>();
      List<ImportProps> importPropsList = new List<ImportProps>();
      try
      {
        string filePath = System.Web.HttpContext.Current.Server.MapPath("~\\" + ScConstants.Misc.translationFolderName + "\\" + ScConstants.Misc.filesForImportFolderName + "\\");
        int count = 1;

        foreach (XtmJob xtmJob in xtmJobList)
        {
          string zipFileName = filePath + "file" + count.ToString() + ".zip";
          string xmlFileName = filePath + "file" + count.ToString() + ".xml";

          File.WriteAllBytes(zipFileName, xtmJob.TranslationFileInBytes);
          using (ZipArchive archive = ZipFile.OpenRead(zipFileName))
          {
            IReadOnlyCollection<ZipArchiveEntry> zipEntries = archive.Entries;
            foreach (ZipArchiveEntry entry in zipEntries)
            {
              XmlDocument xmlDoc = new XmlDocument();
              entry.ExtractToFile(xmlFileName, true);
              xmlDoc.Load(xmlFileName);
              ImportProps importProps = new ImportProps()
              {
                TranslatedXmlDocument = xmlDoc,
                TargetLanguage = xtmJob.TargetLanguage
              };
              //xmlDocuments.Add(xmlDoc);
              importPropsList.Add(importProps);
            }
          }
          XtmSettingsItem xtmSettingsItem = new XtmSettingsItem();
          if (!xtmSettingsItem.IsInDebugMode)
          {
            File.Delete(zipFileName);
            File.Delete(xmlFileName);
          }          
          count++;
        }
      }
      catch (Exception e)
      {
        scLogging.WriteError(e.Message);
      }
      
      return importPropsList;
    }

    public bool ReadFromXml(XmlDocument xmlDocument, string targetLanguage, string sourceLanguage, string xtmProjectId)
    {
      ScLogging scLogging = new ScLogging();
      try
      {
        XmlNode rootElement = xmlDocument.SelectSingleNode(ScConstants.XmlNodes.XmlRoot);
        foreach (XmlNode node in rootElement)
        {
          //Validate node before trying to create the translated item
          ValidateXml validateXml = new ValidateXml();

          if (validateXml.Validate(node))
          {
            if (node.Name.Equals(ScConstants.XmlNodes.XmlRootElement))
            {
              Item translatedItem = CreateNewTranslatedVersion(node, targetLanguage);

              //Create metatranslationitem + version
              MetaTranslationVersion metaTranslationVersion = new MetaTranslationVersion();
              metaTranslationVersion.CreateMetaTranslationVersion(translatedItem, sourceLanguage, xtmProjectId);
            }
          }
          else
          {
            scLogging.WriteWarn("Xml for the item with ID " + node.Attributes[ScConstants.XmlNodes.XmlAttributeId].Value + "was inconsistent and a new translated version was not created");
          }
          //if (node.Name.Equals(ScConstants.XmlNodes.XmlAttributeFieldName))
          //{

          //}
        }
        return true;
      }
      catch (Exception ex)
      {        
        scLogging.WriteError(ex.Message);
        return false;
      }
    }

    private Item CreateNewTranslatedVersion(XmlNode node, string targetLanguage)
    {
      string sitecoreItemId = node.Attributes[ScConstants.XmlNodes.XmlAttributeId].Value;
      string sourceLanguage = node.Attributes[ScConstants.XmlNodes.XmlAttributeSourceLanguage].Value;
      string addedBy = node.Attributes[ScConstants.XmlNodes.XmlAttributeAddedBy].Value;
      
      Mapping mapping = new Mapping();
      targetLanguage = mapping.XtmLanguageToSitecoreLanguage(targetLanguage);

      Language SitecoreLanguage = Language.Parse(targetLanguage);

      Item translationItem = ScConstants.SitecoreDatabases.MasterDb.GetItem(sitecoreItemId, SitecoreLanguage);
      Item newVersion;

      using (new SecurityDisabler())
      {
        newVersion = translationItem.Versions.AddVersion();
      }

      XtmSettingsItem xtmSettings = new XtmSettingsItem();

      List<UpdateItem> fieldObjects = new List<UpdateItem>();

      string dateNow = DateTime.Now.ToString("yyyyMMddTHHmmssZ");

      fieldObjects.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreStandardFieldNames.Workflow, FieldValue = xtmSettings.WorkflowId });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreStandardFieldNames.WorkflowState, FieldValue = xtmSettings.InitialWorfklowStateId });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreXtmTemplateFieldIDs.Translated, FieldValue = "1" });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreXtmTemplateFieldIDs.TranslatedDate, FieldValue = DateTime.Now.ToString("yyyyMMddTHHmmss") });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreXtmTemplateFieldIDs.TranslatedFrom, FieldValue = sourceLanguage });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreXtmTemplateFieldIDs.InTranslation, FieldValue = "0" });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreXtmTemplateFieldIDs.AddedToTranslateionBy, FieldValue = addedBy });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreStandardFieldNames.CreatedBy, FieldValue = "XTM" });

      foreach (XmlNode fieldNode in node)
      {
        string fieldType = fieldNode.Attributes[ScConstants.XmlNodes.XmlAttributeFieldType].Value;
        if (fieldType.Equals("Image"))
        {
          XmlNode defaultValue = fieldNode.Attributes[ScConstants.XmlNodes.XmlAttributeDefaultValue];
          if (defaultValue != null)
          {
            string isDefaultText = fieldNode.Attributes[ScConstants.XmlNodes.XmlAttributeDefaultValue].Value;
            if (bool.Parse(isDefaultText))
            {
              //TODO check the return value! find out how to extract the alt text and update on the media item instead of the instance of the image
            }
          }
        }
        else
        {
          string fieldName = fieldNode.Attributes[ScConstants.XmlNodes.XmlAttributeFieldName].Value;
          if (!fieldName.Equals(""))
          {
            UpdateItem field = new UpdateItem();
            field.FieldIdOrName = fieldName;
            field.FieldValue = fieldNode.InnerText;
            fieldObjects.Add(field);
          }
        }           
      }

      UpdateItem updateItem = new UpdateItem();
      updateItem.Update(newVersion, fieldObjects);

      return (translationItem);
    }

    //private string TransformLangauge(string languageToTransform)
    //{
    //  Item languageMappingFolder = masterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsLanguageMappingFolder);

    //  foreach (Item langMapping in languageMappingFolder.GetChildren())
    //  {
    //    string xtmLang = ScConstants.SitecoreDatabases.MasterDb.GetItem(langMapping[ScConstants.SitecoreFieldIds.XtmLanguageName]).Name;
    //    if (xtmLang.Equals(languageToTransform))
    //    {
    //      languageToTransform = langMapping[ScConstants.SitecoreFieldIds.SitecoreLanguageName];
    //    }
    //  }
    //  return languageToTransform;
    //}
  }
}
