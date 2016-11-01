using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel;
using System.Collections.Generic;
using System.Text;

namespace LjungbergIt.Xtm.Connector.AddForTranslation
{
    class TranslationQueue
    {
        Database masterDatabase = ScConstants.SitecoreDatabases.MasterDb;

        public string AddToQueue(Item itemToTranslate, string sourceLangauge, string targetLanguage, string xtmTemplate, string addedBy)
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

                //DELETE Item translationQueueFolder = masterDatabase.GetItem(ScConstants.SitecoreIDs.TranslationQueueFolder);
                
                StringBuilder sbFolderName = new StringBuilder();
                sbFolderName.Append(sourceLangauge);
                sbFolderName.Append("_to_");
                sbFolderName.Append(targetLanguage);
                sbFolderName.Append("_");
                sbFolderName.Append(xtmTemplateName);
                GetLanguageQueueFolder getFolder = new GetLanguageQueueFolder();
                TranslationProperties translationProperties = new TranslationProperties { SourceLanguage = sourceLangauge, TargetLanguage = targetLanguage, XtmTemplate = xtmTemplate };
                Item langFolder = getFolder.GetFolder(sbFolderName.ToString(), translationProperties);
                List<string> queueItemNames = new List<string>();
                foreach (Item queueItem in langFolder.GetChildren())
                {
                    queueItemNames.Add(queueItem.Name);
                }

                Utils utils = new Utils();
                string itemName = string.Format("{0}_{1}_{2}", utils.FormatItemName(itemToTranslate.ID.ToString()), targetLanguage, "VERSION");

                if (queueItemNames.Contains(itemName))
                {
                    returnString = string.Format("{0} have already been added to the queue with {1} as source langauge and {2} as target language", itemToTranslate.Name, sourceLangauge, targetLanguage);
                }
                else
                {
                    //TODO use the updateitem class instead
                    Sitecore.Diagnostics.Log.Info("XtmConnector: adding queue item to the folder with name: " + itemName, this);
                    Item translationQueueItem = langFolder.Add(itemName, ScConstants.SitecoreTemplates.TranslationQueueItemTemplate);                    
                    
                        translationQueueItem.Editing.BeginEdit();
                        translationQueueItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_ItemId] = itemToTranslate.ID.ToString();
                        //translationQueueItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_Version] = args.ItemVersion;
                        translationQueueItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_TranslateTo] = targetLanguage;
                        translationQueueItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_MasterLanguage] = sourceLangauge;
                        translationQueueItem[ScConstants.SitecoreFieldIds.TranslationQueueItem_AddedBy] = addedBy;
                        translationQueueItem.Editing.EndEdit();
                    
                    returnString = string.Format("{0} have been added for translation from {1} to {2}", itemToTranslate.Name, sourceLangauge, targetLanguage);
                }
            }
            return returnString;
        }
    }
}
