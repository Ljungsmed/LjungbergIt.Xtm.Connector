
using LjungbergIt.Xtm.Connector.Export;
using LjungbergIt.Xtm.Connector.LanguageHandling;
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
      //if (args.IsPostBack)
      //{
      //if (args.Result == "yes")
      //{

      Item contextItem = masterDatabase.GetItem(args.ItemId);
      XtmBaseTemplate xtmBaseTemplate = new XtmBaseTemplate(null); 

      StringBuilder errorMessage = new StringBuilder();
      
      bool showAddForTranslation = false;

      if (xtmBaseTemplate.CheckForBaseTemplate(contextItem))
      {
        CheckboxField inTranslation = contextItem.Fields[ScConstants.SitecoreXtmTemplateFieldIDs.InTranslation];
        if (!inTranslation.Checked)
        {
          showAddForTranslation = true;          
        }
        else
        {
          errorMessage.Append("The selected content item is already being translated");
          SheerResponse.Alert(errorMessage.ToString());
        }
      }
      else
      {
        showAddForTranslation = true;
        errorMessage.Append("The selected content item is not available for translation. If this is a mistake, contact your XTM administrator. You can still add sub items for translation");
        SheerResponse.Alert(errorMessage.ToString());
      }

      if (showAddForTranslation)
      {
        Sitecore.Text.UrlString buttonUrl = new Sitecore.Text.UrlString("/XtmFiles/AddItemForTranslation.aspx");
        buttonUrl.Append("id", args.ItemId);
        SheerResponse.ShowModalDialog(buttonUrl.ToString(), "800", "600", "testing message", false);
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