using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Collections;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LjungbergIt.Xtm.Connector.XtmFiles
{
    public partial class TranslationDetailedView : System.Web.UI.Page
    {
        public string MyProperty { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ItemList currentQueue = new ItemList();
            Item queueFolder = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueFolder);
            foreach (Item queueItem in queueFolder.GetChildren())
            {

            }
        }
    }
}