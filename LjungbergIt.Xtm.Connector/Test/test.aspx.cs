using LjungbergIt.Xtm.Connector.Commands;
using LjungbergIt.Xtm.Connector.Import;
using LjungbergIt.Xtm.Connector.Export;
using LjungbergIt.Xtm.Webservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace LjungbergIt.Xtm.Connector.Test
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(Object sender, EventArgs e)
        {
            //XtmHandleTranslatedContent translation = new XtmHandleTranslatedContent();
            ImportFromXml import = new ImportFromXml();

            import.CreateTranslatedContentFromProgressFolder();
            //ConvertToXml convert = new ConvertToXml();
            //convert.Transform();
            //litInfo.Text = translation.GetFileInBytes(3832);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            GetXtmTemplates getTemplates = new GetXtmTemplates();
            getTemplates.TemplatesFromXtm();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

            //XtmWebserviceAccess xtm = new XtmWebserviceAccess();
            XtmStartTranslation start = new XtmStartTranslation();
            start.start();
            litInfo.Text = "DONE";
        }
    }
}