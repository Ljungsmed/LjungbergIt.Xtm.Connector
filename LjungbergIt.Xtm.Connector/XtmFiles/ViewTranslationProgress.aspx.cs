using LjungbergIt.Xtm.Connector.LanguageHandling;
using LjungbergIt.Xtm.Connector.Import;
using LjungbergIt.Xtm.Webservice;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace LjungbergIt.Xtm.Connector.XtmFiles
{
  public partial class ViewTranslationProgress : System.Web.UI.Page
  {
    protected void Page_Init(object sender, EventArgs e)
    {
      lwProgress.ItemCommand += new EventHandler<ListViewCommandEventArgs>(lwProgress_ItemCommand);
    }

    void lwProgress_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
      string info = "General error";
      string projectIdString = (String)e.CommandArgument;

      long projectId;
      bool projectIdOk = long.TryParse(projectIdString, out projectId);
      if (projectIdOk)
      {
        ImportFromXml import = new ImportFromXml();

        Item finnishedProgressItem = null;
        Item progressFolder = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.TranslationInProgressFolder);
        foreach (Item progressItem in progressFolder.GetChildren())
        {
          if (progressItem.Name.Equals(projectId.ToString()))
          {
            finnishedProgressItem = progressItem;
          }
        }

        bool success = import.CreateTranslatedContent(projectId, finnishedProgressItem);
        lwProgress.DataSource = BuildProjectlist();
        lwProgress.DataBind();
        if (success)
        {
          info = "You succesfully imported the translated content. See your workbox for further details";
        }
        else
        {
          info = "There was a problem with the translated project with id " + projectId + " in XTM, please contact your XTM admin";
        }
      }
      else
      {
        info = "There is something wrong with the project id";
      }

      litTest.Text = info;

    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        List<XtmProject> allProjects = BuildProjectlist();

        List<XtmProject> projectsNoErrors = new List<XtmProject>();
        List<XtmProject> projectsErrors = new List<XtmProject>();
        foreach (XtmProject project in allProjects)
        {
          if (!project.ProjectError)
          {
            projectsNoErrors.Add(project);
          }
          else
          {
            projectsErrors.Add(project);
          }
        }

        lwProgress.DataSource = projectsNoErrors;
        lwProgress.DataBind();

        if (projectsErrors.Count == 0)
        {
          ProjectErrorList.Visible = false;
        }
        else
        {
          rptProjectErrors.DataSource = projectsErrors;
          rptProjectErrors.DataBind();
        }
        
      }
    }

    protected List<XtmProject> BuildProjectlist()
    {
      Item progressFolder = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.TranslationInProgressFolder);
      List<long> xtmProjectIds = new List<long>();
      foreach (Item progressItem in progressFolder.GetChildren())
      {
        try
        {
          long xtmId = long.Parse(progressItem[ScConstants.SitecoreFieldIds.XtmProjectId]);
          xtmProjectIds.Add(xtmId);
        }
        catch (Exception ex)
        {
          litInfo.Text = ex.Message.ToString();
        }
      }

      //Item settingsItem = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsItem);
      //string webServiceEndPoint = settingsItem[ScConstants.SitecoreFieldIds.XtmSettingsEndpoint];
      XtmWebserviceProperties xtmWebserviceProperties = new XtmWebserviceProperties();

      XtmProject xtmProject = new XtmProject();
      LoginProperties login = new LoginProperties();
      List<XtmProject> projects = xtmProject.GetProjects(xtmProjectIds, login.ScClient, login.ScUserId, login.ScPassword, xtmWebserviceProperties.WebserviceEndpoint, xtmWebserviceProperties.IsHttps);

      return projects;
    }

    protected bool RenderImportButton(string stringStatus)
    {
      bool status = false;

      if (stringStatus != null)
      {
        if (stringStatus.Contains("FINISHED"))
        {
          status = true;
        }
      }     

      return status;
    }
  }
}