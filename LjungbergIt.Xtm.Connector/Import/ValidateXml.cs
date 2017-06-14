using LjungbergIt.Xtm.Connector.LanguageHandling;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace LjungbergIt.Xtm.Connector.Import
{
  public class ValidateXml
  {
    public bool Validate(XmlNode node)
    {
      bool validated = false;

      string sitecoreItemId = node.Attributes[ScConstants.XmlNodes.XmlAttributeId].Value;
      Item translationItem = ScConstants.SitecoreDatabases.MasterDb.GetItem(sitecoreItemId);

      if (translationItem != null)
      {
        int fieldCount = translationItem.Fields.Count(x => !x.Name.Contains("__") && !x.Name.Contains("XTM_"));
        if (fieldCount > 0)
        {
          validated = true;
        }
      }
      return validated;
    }
  }
}