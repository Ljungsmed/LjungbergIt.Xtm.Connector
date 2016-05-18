
using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Connector.Import;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;

namespace LjungbergIt.Xtm.Connector.Pipelines
{
    class XtmProcessor
    {
        Database masterDatabase = ScConstants.SitecoreDatabases.MasterDb;

        public void AddItemForTranslation(XtmPipelineArgs args)
        {
            if (args.IsPostBack)
            {
                if (args.Result == "yes")
                {
                    //Sitecore.Context.ClientPage.ClientResponse.Alert(args.ItemId);
                    
                    Item translationQueueFolder = masterDatabase.GetItem(ScConstants.SitecoreIDs.TranslationQueueFolder);
                    string masterLanguageID = masterDatabase.GetItem(ScConstants.SitecoreIDs.XtmSettingsItem)[ScConstants.SitecoreFieldNames.Settings_MasterLanguage];
                    string masterLanguage = masterDatabase.GetItem(masterLanguageID)[ScConstants.SitecoreFieldNames.SitecoreLanguageItem_Iso];

                    Utils utils = new Utils();
                    string itemName = string.Format("{0}_{1}_{2}", utils.FormatItemName(args.ItemId), args.ItemLanguage, args.ItemVersion);

                    //TODO add item anyway!
                    if (masterLanguage.Equals(args.ItemLanguage))
                    {
                        SheerResponse.Alert("The language chosen is the same as the master language");
                    }
                    else
                    {
                        GetLanguageQueueFolder getFolder = new GetLanguageQueueFolder();
                        Item langFolder = getFolder.GetFolder(args.ItemLanguage);
                        //Item langFolder = getFolder.GetFolder(DateTime.Now.ToString("yyyyMMdd"));

                        List<string> queueItemNames = new List<string>();
                        foreach (Item queueItem in langFolder.GetChildren())
                        {
                            queueItemNames.Add(queueItem.Name);
                        }

                        if (queueItemNames.Contains(itemName))
                        {
                            SheerResponse.Alert("That item has already been added to the queue in that language");
                        }

                        else
                        {
                            Item translationQueueItem = langFolder.Add(itemName, ScConstants.SitecoreTemplates.TranslationQueueItemTemplate);
                            using (new Sitecore.SecurityModel.SecurityDisabler())
                            {                                
                                translationQueueItem.Editing.BeginEdit();
                                translationQueueItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_ItemId] = args.ItemId;
                                translationQueueItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_Version] = args.ItemVersion;
                                translationQueueItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_TranslateTo] = args.ItemLanguage;
                                translationQueueItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_MasterLanguage] = masterLanguage;
                                translationQueueItem.Editing.EndEdit();
                            }
                            SheerResponse.Alert("Item added for translation");
                            //TODO set focus to the original item, not the one being created
                            //SheerResponse.Focus(args.ItemId.ToString());
                        }
                    }                    
                }
                else
                    args.AbortPipeline();
            }
            else
            {
                //SheerResponse.ShowModalDialog()
                //SheerResponse.Input("test", "testc");
                SheerResponse.Confirm("Add all fields for translation?");
                args.WaitForPostBack();
            }
            

            //SheerResponse.Input("test", "test");
            //Sitecore.Context.ClientPage.ClientRequest.DialogWidth.
            
        }
    }
}
