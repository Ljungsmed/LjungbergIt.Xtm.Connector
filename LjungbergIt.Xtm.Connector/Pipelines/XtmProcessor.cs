
using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Web.UI.Sheer;
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
                CheckboxField inTranslation = contextItem.Fields[ScConstants.SitecoreXtmTemplateFieldIDs.InTranslation];
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
                SheerResponse.Alert(errorMessage.ToString());
            }
        }

        public void ViewTranslationProgress(XtmPipelineArgs args)
        {
            SheerResponse.ShowModalDialog("/XtmFiles/ViewTranslationProgress.aspx", "1200", "800", "testing my message", false);
        }

        public void TranslationDetailedView(XtmPipelineArgs args)
        {
            SheerResponse.ShowModalDialog("/XtmFiles/TranslationDetailedView.aspx", "1200", "800", "testing my message", false);
        }

        public void ViewDocumentation(XtmPipelineArgs args)
        {
            SheerResponse.ShowModalDialog("/XtmFiles/ViewDocumentation.aspx", "1200", "800", "testing my message", false);
        }
    }
}