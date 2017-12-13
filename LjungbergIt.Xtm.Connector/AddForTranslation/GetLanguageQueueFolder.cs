using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Collections;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel;

namespace LjungbergIt.Xtm.Connector.Helpers
{
  class GetLanguageQueueFolder
  {
    public Item GetFolder(string folderName, TranslationProperties translationProperties)
    {
      Item returnItem = null;

      //using (new SecurityDisabler())
      //{
      Item queueFolder = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueFolder);
      //TODO: change foreach to use LINQ instead
      foreach (Item folderItem in queueFolder.GetChildren())
      {
        if (folderItem.Name.Equals(folderName))
        {
          returnItem = folderItem;
        }
      }

      if (returnItem == null)
      {
        returnItem = CreateFolder(queueFolder, folderName, translationProperties);
      }
      //}

      return returnItem;
    }
    private Item CreateFolder(Item queueFolderItem, string folderName, TranslationProperties translationProperties)
    {
      Item langFolderItem = null;
      if (translationProperties.XtmTemplate.Equals("NONE"))
      {
        translationProperties.XtmTemplate = string.Empty;
      }

      //TODO use the UpdateItem helper instead
      using (new SecurityDisabler())
      {
        //Sitecore.Diagnostics.Log.Info("XtmConnector: creating folder with name " + folderName, this);
        langFolderItem = queueFolderItem.Add(folderName, ScConstants.SitecoreTemplates.TranslationQueueLanguageFolderTemplate);
        langFolderItem.Editing.BeginEdit();
        langFolderItem[ScConstants.XtmQueueProjectTemplateFolder.SourceLanguage] = translationProperties.SourceLanguage;
        langFolderItem[ScConstants.XtmQueueProjectTemplateFolder.XTMProjectName] = folderName;
        langFolderItem[ScConstants.XtmQueueProjectTemplateFolder.DueDate] = translationProperties.DueDate;

        Mapping mapping = new Mapping();
        ItemList languageItems = mapping.LanguageNamesToItems(translationProperties.TargetLanguages);
        MultilistField targetLanguagesField = langFolderItem.Fields[ScConstants.XtmQueueProjectTemplateFolder.TargetLanguages];
        foreach (Item languageItem in languageItems)
        {
          targetLanguagesField.Add(languageItem.ID.ToString());
        }
        langFolderItem[ScConstants.XtmQueueProjectTemplateFolder.XtmTemplate] = translationProperties.XtmTemplate;
        langFolderItem.Editing.EndEdit();
      }

      return langFolderItem;
    }
  }
}
