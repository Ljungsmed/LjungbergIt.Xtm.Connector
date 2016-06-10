using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Connector.Helpers
{
   public class LoginProperties
    {
        Item settingsItem = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsItem);

        string scClient;
        public string ScClient
        {            
            get
            {
                return scClient = settingsItem[ScConstants.SitecoreFieldIds.XtmSettingsClient];
            }
        }

        long scUserid;
        public long ScUserId
        {
            get
            {
                return scUserid = long.Parse(settingsItem[ScConstants.SitecoreFieldIds.XtmSettingsUserId]);
            }
        }

        string scPassword;
        public string ScPassword
        {
            get
            {
                return scPassword = settingsItem[ScConstants.SitecoreFieldIds.XtmSettingsPasword];
            }
        }

        long scCustomer;
        public long ScCustomer
        {
            get
            {
                return scCustomer = long.Parse(settingsItem[ScConstants.SitecoreFieldIds.XtmSettingsCustomer]);
            }
        }

    }
}
