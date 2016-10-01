using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace LjungbergIt.Xtm.Connector.Helpers
{
    class XtmWebserviceProperties
    {
        public bool IsHttps { get; set; }
        public string WebserviceEndpoint  { get; set; }

        public XtmWebserviceProperties()
        {
            Item xtmSettingsItem = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsItem);
            CheckboxField httpsField = xtmSettingsItem.Fields[ScConstants.XtmSettingsTemplate.https];
            if (httpsField.Checked)
            {
                IsHttps = true;
            }
            else
            {
                IsHttps = false;
            }

            WebserviceEndpoint = xtmSettingsItem[ScConstants.XtmSettingsTemplate.Endpoint];

        }
    }
}
