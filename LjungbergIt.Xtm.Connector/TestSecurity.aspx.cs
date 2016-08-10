using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LjungbergIt.Xtm.Connector
{
    public partial class TestSecurity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (new SecurityDisabler())
            {
                test.Text = Sitecore.Data.Database.GetDatabase("master").GetItem("{12AC7B4A-C1EB-4F34-8DAE-D67BFB7E9883}").Name;
            }
                
        }
    }
}