using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Helpers
{
  public class Label
  {
    public string LabelTitle { get; set; }
    public string LabelText { get; set; }

    string TemplateLabelTitleFieldId = "{279E5E85-8525-4EE8-B399-0096ED5A7D04}";
    string TemplateLabelTextFieldId = "{7B71B74A-B3D3-4D82-AC7A-21CD60287D49}";
    public Label(Item item)
    {
      LabelTitle = item[TemplateLabelTitleFieldId];
      LabelText = item[TemplateLabelTextFieldId];
    }
  }
}