
using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Connector.Import;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.Text;

namespace LjungbergIt.Xtm.Connector.Pipelines
{
    class XtmProcessor
    {
        Database masterDatabase = ScConstants.SitecoreDatabases.MasterDb;

        public void AddItemForTranslation(XtmPipelineArgs args)
        {
            bool hasXtmBaseTemplate = false;
            //if (args.IsPostBack)
            //{
            //if (args.Result == "yes")
            //{

            //TODO check if base template is XTMBase
            Item contextItem = masterDatabase.GetItem(args.ItemId);

            foreach (TemplateItem baseTemplate in contextItem.Template.BaseTemplates)
            {
                if (baseTemplate.ID == ScConstants.SitecoreTemplates.XtmBaseTemplate)
                {
                    hasXtmBaseTemplate = true;
                }
            }

            StringBuilder errorMessage = new StringBuilder();

            if (hasXtmBaseTemplate)
            {
                CheckboxField inTranslation = contextItem.Fields[ScConstants.SitecoreFieldIds.XtmBaseTemplateInTranslation];
                if (!inTranslation.Checked)
                {

                    Sitecore.Text.UrlString buttonUrl = new Sitecore.Text.UrlString("/XtmFiles/AddItemForTranslation.aspx");
                    buttonUrl.Append("id", args.ItemId);

                    SheerResponse.ShowModalDialog(buttonUrl.ToString(), "800", "600", "testing message", false);
                }
                else
                {
                    errorMessage.Append("The selected content item is already being translated");
                    SheerResponse.Alert(errorMessage.ToString());
                }

            }
            else
            {   
                errorMessage.Append("The selected content item is not available for translation. If this is a mistake, contact your XTM administrator");
                
                //TODO create the error message and output the XTM administrator from the settings item
                SheerResponse.Alert(errorMessage.ToString());
            }

            

            //Item translationQueueFolder = masterDatabase.GetItem(ScConstants.SitecoreIDs.TranslationQueueFolder);
            //string masterLanguageID = masterDatabase.GetItem(ScConstants.SitecoreIDs.XtmSettingsItem)[ScConstants.SitecoreFieldNames.Settings_MasterLanguage];
            //string masterLanguage = masterDatabase.GetItem(masterLanguageID)[ScConstants.SitecoreFieldNames.SitecoreLanguageItem_Iso];

            //Utils utils = new Utils();
            //string itemName = string.Format("{0}_{1}_{2}", utils.FormatItemName(args.ItemId), args.ItemLanguage, args.ItemVersion);

            ////TODO add item anyway!
            //if (masterLanguage.Equals(args.ItemLanguage))
            //{
            //    SheerResponse.Alert("The language chosen is the same as the master language");
            //}
            //else
            //{
            //    GetLanguageQueueFolder getFolder = new GetLanguageQueueFolder();
            //    Item langFolder = getFolder.GetFolder(args.ItemLanguage);
            //    //Item langFolder = getFolder.GetFolder(DateTime.Now.ToString("yyyyMMdd"));

            //    List<string> queueItemNames = new List<string>();
            //    foreach (Item queueItem in langFolder.GetChildren())
            //    {
            //        queueItemNames.Add(queueItem.Name);
            //    }

            //    if (queueItemNames.Contains(itemName))
            //    {
            //        SheerResponse.Alert("That item has already been added to the queue in that language");
            //    }

            //    else
            //    {
            //        Item translationQueueItem = langFolder.Add(itemName, ScConstants.SitecoreTemplates.TranslationQueueItemTemplate);
            //        using (new Sitecore.SecurityModel.SecurityDisabler())
            //        {                                
            //            translationQueueItem.Editing.BeginEdit();
            //            translationQueueItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_ItemId] = args.ItemId;
            //            translationQueueItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_Version] = args.ItemVersion;
            //            translationQueueItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_TranslateTo] = args.ItemLanguage;
            //            translationQueueItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_MasterLanguage] = masterLanguage;
            //            translationQueueItem.Editing.EndEdit();
            //        }
            //        SheerResponse.Alert("Item added for translation");
            //TODO set focus to the original item, not the one being created
            //SheerResponse.Focus(args.ItemId.ToString());
        }
    }
    //}
    //else
    //    args.AbortPipeline();
    //}
    //else
    //{
    //SheerResponse.ShowModalDialog()
    //SheerResponse.Input("test", "testc");
    //SheerResponse.Confirm("Add all fields for translation?");
    //args.WaitForPostBack();
    //}


    //SheerResponse.Input("test", "test");
    //Sitecore.Context.ClientPage.ClientRequest.DialogWidth.

}