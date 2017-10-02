using LjungbergIt.Xtm.Connector.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Export
{
    public class ExcludeFields
    {
        public string ExcludeFieldIds { get; set; }

        public ExcludeFields()
        {
            XtmSettingsItem settingsItem = new XtmSettingsItem();
            ExcludeFieldIds = settingsItem.ExcludedFieldsIds;
        }
    }    
}