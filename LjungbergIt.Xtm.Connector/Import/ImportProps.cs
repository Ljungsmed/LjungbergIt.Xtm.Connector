using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace LjungbergIt.Xtm.Connector.Import
{
  public class ImportProps
  {
    public XmlDocument TranslatedXmlDocument { get; set; }
    public string TargetLanguage { get; set; }
  }
}