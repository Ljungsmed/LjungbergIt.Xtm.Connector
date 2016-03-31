
using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data.Items;
using Sitecore.Web.UI.Sheer;

namespace LjungbergIt.Xtm.Connector.Pipelines
{
    class XtmProcessor
    {
        public void ChooseMasterLanguage(XtmPipelineArgs args)
        {
            if (args.IsPostBack)
            {
                if (args.Result == "yes")
                {
                    //Sitecore.Context.ClientPage.ClientResponse.Alert(args.ItemId);
                    Item translationQueueFolder =  ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueFolder);
                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {
                        Utils utils = new Utils();
                        string itemName = string.Format("{0}_{1}_{2}", utils.FormatItemName(args.ItemId), args.ItemLanguage, args.ItemVersion);
                        Item translationQueueItem = translationQueueFolder.Add(itemName, ScConstants.SitecoreTemplates.TranslationQueueItemTemplate);
                        translationQueueItem.Editing.BeginEdit();
                        translationQueueItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_ItemId] = args.ItemId;
                        translationQueueItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_Version] = args.ItemVersion;
                        translationQueueItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_TranslateTo] = args.ItemLanguage;
                        translationQueueItem.Editing.EndEdit();
                    }

                    SheerResponse.Alert("Item added for translation");
                }
                else
                    args.AbortPipeline();
            }
            else
            {
                //SheerResponse.Input("test", "testc");
                SheerResponse.Confirm("Add all fields for translation?");
                args.WaitForPostBack();
            }
            

            //SheerResponse.Input("test", "test");
            //Sitecore.Context.ClientPage.ClientRequest.DialogWidth.
            
        }
    }
}
