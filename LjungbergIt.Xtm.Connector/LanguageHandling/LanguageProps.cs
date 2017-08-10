using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Helpers
{
  public class LanguageProps
  {
    private static readonly string LanguageFolderId = "{64C4F646-A3FA-4205-B98E-4DE2C609B60F}";
    public Item LanguageFolderItem { get; set; }

    public LanguageProps()
    {
      LanguageFolderItem = ScConstants.SitecoreDatabases.MasterDb.GetItem(LanguageFolderId);
    }

  }
}