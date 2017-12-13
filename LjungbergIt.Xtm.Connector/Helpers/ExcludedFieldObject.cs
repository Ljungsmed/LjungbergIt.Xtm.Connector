using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Collections;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Helpers
{
  public class ExcludedFieldObject
  {
    //public string ExcludeFieldIds { get; set; }
    public string FieldId { get; set; }
    public string FieldName { get; set; }
    public string TemplateSection { get; set; } 
    public string TemplateName { get; set; }
    public string TemplatePath { get; set; }

    public ExcludedFieldObject()
    {
    }
  }
}