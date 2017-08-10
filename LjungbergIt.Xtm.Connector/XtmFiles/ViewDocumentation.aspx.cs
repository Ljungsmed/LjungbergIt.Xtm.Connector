using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;

namespace LjungbergIt.Xtm.Connector.XtmFiles
{
    public partial class ViewDocumentation : System.Web.UI.Page
    {
        public string DocumentationHeading { get; set; }
        public string DocumentationText { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Item documentationFolder = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsDocumentationFolder);
            List<ViewDocumentation> documentationSections = new List<ViewDocumentation>();
            foreach (Item documentationItem in documentationFolder.GetChildren())
            {
                ViewDocumentation section = new ViewDocumentation()
                {
                    DocumentationHeading = documentationItem[ScConstants.XtmDocumentationSectionTemplate.SectionHeading],
                    DocumentationText = documentationItem[ScConstants.XtmDocumentationSectionTemplate.SectionText]
                };
                documentationSections.Add(section);

                rptDocumentation.DataSource = documentationSections;
                rptDocumentation.DataBind();
            }

        }
    }
}