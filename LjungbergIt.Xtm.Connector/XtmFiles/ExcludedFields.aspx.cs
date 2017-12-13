using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LjungbergIt.Xtm.Connector.XtmFiles
{
  public partial class ExcludedFields : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e) 
    {
      XtmSettingsItem settingsItem = new XtmSettingsItem();
      Item[] ExcludedItems = settingsItem.GetExcludedFieldItems();
      List<ExcludedFieldObject> excludedFieldsObjects = new List<ExcludedFieldObject>();
      foreach (Item excludedFieldItem in ExcludedItems)
      {
        Item templateItem = excludedFieldItem.Parent.Parent;

        excludedFieldsObjects.Add(new ExcludedFieldObject {
          FieldId = excludedFieldItem.ID.ToString(),
          FieldName = excludedFieldItem.Name,
          TemplateSection = excludedFieldItem.Parent.Name,
          TemplateName = templateItem.Name,
          TemplatePath = templateItem.Paths.Path
        });
      }
      rptExlcudedFields.DataSource = excludedFieldsObjects;
      rptExlcudedFields.DataBind();      
    }
  }
}