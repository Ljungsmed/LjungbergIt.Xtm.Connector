﻿using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Connector.Import;
using LjungbergIt.Xtm.Webservice;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
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
            litTest.Text = "You clicked the " + (String)e.CommandArgument + " button";

            string projectIdString = (String)e.CommandArgument;

            long projectId;
            bool projectIdOk = long.TryParse(projectIdString, out projectId);
            if (projectIdOk)
            {
                ImportFromXml import = new ImportFromXml();              
                import.CreateTranslatedContent(projectId);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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

                XtmProject xtmProject = new XtmProject();
                List<XtmProject> projects = xtmProject.GetProjectProperties(xtmProjectIds);

                lwProgress.DataSource = projects;
                lwProgress.DataBind();
            }
        }

        protected bool RenderImportButton(string stringStatus)
        {
            bool status = false;
            
            if(stringStatus.Contains("FINISHED"))
            {
                status = true;
            }

            return status;
        }
    }
}