using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Helpers
{
  public class CheckListHelper
  {
    public TranslationItem GetIds(string stringOfIds)
    {
      int separator = stringOfIds.IndexOf("_");
      if (separator != -1)
      {
        string itemForTranslationId = stringOfIds.Substring(0, separator);
        Item itemForTranslation = ScConstants.SitecoreDatabases.MasterDb.GetItem(itemForTranslationId);
        string relatedItemId = stringOfIds.Substring(separator + 1);
        Item relatedItem = ScConstants.SitecoreDatabases.MasterDb.GetItem(relatedItemId);
        if (itemForTranslation != null && relatedItem != null)
        {
          return ( new TranslationItem() { sitecoreItem = itemForTranslation, RelatesTo = relatedItem });
        }
      }
      return null;
    }
  }
}