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
using Sitecore.Collections;
using LjungbergIt.Xtm.Connector.Search;
using Sitecore.Data.Items;
using LjungbergIt.Xtm.Connector.Export;
using LjungbergIt.Xtm.Connector.Helpers;

namespace LjungbergIt.Xtm.Connector.Test
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //XtmLanguages lang = new XtmLanguages();
            //lang.CreateAllXtmLanguages();
        }

        protected void Button1_Click(Object sender, EventArgs e)
        {
      ItemSearch itemSearch = new ItemSearch();
      ItemList returnList = itemSearch.FindItems("{DF5AC8B4-9C6A-42EA-9E39-3D6C71C0C5EA}", "itemid", "{C9CFB071-5A6C-47E2-A9C9-03BE48E4C7B1}");

      string itemsString = "No items found";

      foreach (Item item in returnList)
      {
        itemsString = item.Name;
      }

      Button1Lit.Text = itemsString;
            //XtmHandleTranslatedContent translation = new XtmHandleTranslatedContent();
            //ImportFromXml import = new ImportFromXml();

            //import.CreateTranslatedContentFromProgressFolder();
            //ConvertToXml convert = new ConvertToXml();
            //convert.Transform();
            //litInfo.Text = translation.GetFileInBytes(3832);

            
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
      //RelatedContent test = new RelatedContent();
      
      //ItemList list = test.GetRelatedContentItems(ScConstants.SitecoreDatabases.MasterDb.GetItem("{7A7319F5-7613-4E03-94C5-88802C7711B3}"));
            //GetXtmTemplates getTemplates = new GetXtmTemplates();
            //getTemplates.TemplatesFromXtm();
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