using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel;
using System.Collections.Generic;
using System.Text;

namespace LjungbergIt.Xtm.Connector.Export
{
  class TranslationQueue
  {
    Database masterDatabase = ScConstants.SitecoreDatabases.MasterDb;

    public string AddToQueue(TranslationItem itemToTranslate, string sourceLangauge, List<string> targetLanguages, string xtmTemplate, string addedBy, string projectName)
    {
      string returnString = "";
      string xtmTemplateName = "NONE";

      using (new SecurityDisabler())
      {
        //set the xtm template values (name and id) if any
        if (xtmTemplate != "NONE")
        {
          Item xtmTemplateItem = masterDatabase.GetItem(xtmTemplate);
          xtmTemplateName = xtmTemplateItem.Name;
          xtmTemplate = xtmTemplateItem.ID.ToString();
        }
        
        GetLanguageQueueFolder getFolder = new GetLanguageQueueFolder();
        TranslationProperties translationProperties = new TranslationProperties { SourceLanguage = sourceLangauge, TargetLanguages = targetLanguages, XtmTemplate = xtmTemplate };
        Item langFolder = getFolder.GetFolder(projectName, translationProperties);
        List<string> queueItemNames = new List<string>();
        foreach (Item queueItem in langFolder.GetChildren())
        {
          queueItemNames.Add(queueItem.Name);
        }

        Utils utils = new Utils();
        //string itemName = string.Format("{0}_{1}", utils.FormatItemName(itemToTranslate.ID.ToString()), targetLanguage);
        string itemName = string.Format("{0}", utils.FormatItemName(itemToTranslate.sitecoreItem.ID.ToString()));

        if (queueItemNames.Contains(itemName))
        {
          returnString = string.Format("{0} have already been added to the project", itemToTranslate.sitecoreItem.Name);
        }
        else
        {
          //TODO use the updateitem class instead
          //Sitecore.Diagnostics.Log.Info("XtmConnector: adding queue item to the folder with name: " + itemName, this);
          Item translationQueueItem = langFolder.Add(itemName, ScConstants.SitecoreTemplates.TranslationQueueItemTemplate);

          translationQueueItem.Editing.BeginEdit();
          translationQueueItem[TranslationQueueItem.XtmTranslationQueueItem.Field_ItemId] = itemToTranslate.sitecoreItem.ID.ToString();
          translationQueueItem[TranslationQueueItem.XtmTranslationQueueItem.Field_MasterLanguage] = sourceLangauge;
          translationQueueItem[TranslationQueueItem.XtmTranslationQueueItem.Field_AddedBy] = addedBy;
          if (itemToTranslate.RelatesTo != null)
          {
            translationQueueItem[TranslationQueueItem.XtmTranslationQueueItem.Field_RelatedItemId] = itemToTranslate.RelatesTo.ID.ToString();
          }
          translationQueueItem.Editing.EndEdit();

          Mapping mapping = new Mapping();

          returnString = string.Format("{0} added for translation to the project", itemToTranslate.sitecoreItem.Name);
        }
      }
      return returnString;
    } 
  }
}
