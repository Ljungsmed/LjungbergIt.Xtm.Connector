using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Helpers
{
  public class XtmSettingsItem
  {
    public string BaseSiteUrl { get; set; }
    public Item HomeItem { get; set; }

    public XtmSettingsItem()
    {
      Database masteDb = ScConstants.SitecoreDatabases.MasterDb;
      Item settingsItem = masteDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsItem);
      BaseSiteUrl = settingsItem["{F8198642-8F33-4CC4-ACD8-0CC3905A480D}"];
      HomeItem = masteDb.GetItem(settingsItem["{12078255-B2F0-4A71-822E-FDA1A75BC941}"]);
    }
  }  
}