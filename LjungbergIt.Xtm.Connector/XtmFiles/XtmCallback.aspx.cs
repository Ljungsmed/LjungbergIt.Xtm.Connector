using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Connector.Import;
using Sitecore.Data.Items;
using System;
using System.Text.RegularExpressions;

namespace LjungbergIt.Xtm.Connector.XtmFiles
{
  public partial class XtmCallback : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      ScLogging scLogging = new ScLogging();      
      string xtmProjectId = Request.QueryString[ScConstants.Misc.CallbackParameter];      
      if (xtmProjectId == null)
      {
        Response.Redirect("/");
      }
      else
      {
        scLogging.WriteInfo("Callback: Callback invoked, starting import of project with id " + xtmProjectId);
        Regex regex = new Regex(@"^[0-9]+$");
        if (regex.IsMatch(xtmProjectId))
        {
          Item progressFolder = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.TranslationInProgressFolder);
          Item ProgressItem = null;
          //TODO get item using search helper
          foreach (Item child in progressFolder.GetChildren())
          {
            if (child.Name.Equals(xtmProjectId))
            {
              ProgressItem = child;
              long xtmProjectIdLong = long.Parse(xtmProjectId);
              ImportFromXml import = new ImportFromXml();
              bool status = import.CreateTranslatedContent(xtmProjectIdLong, ProgressItem);
              if (status)
              {
                scLogging.WriteInfo("Callback: Translated project with id " + xtmProjectId + " was automatically imported");
              }
              else
              {
                scLogging.WriteWarn("Callback: Translated project with id " + xtmProjectId + " was not correctly imported, see errors in log for info");
              }                            
            }
            else
            {
              scLogging.WriteWarn("Callback: ProgessItem not found based on project id " + xtmProjectId);
            }
          }
        }
        else
        {
          Response.Redirect("/");
        }
      }
    }
  }
}