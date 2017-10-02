using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;

namespace LjungbergIt.Xtm.Connector.Helpers
{
  public class XtmSettingsItem
  {
    public string BaseSiteUrl { get; set; }
    public Item HomeItem { get; set; }
    public bool IsInDebugMode { get; set; }
    public string ExcludedFieldsIds { get; set; }
    public string InitialWorfklowStateId { get; set; }
    public string WorkflowId { get; set; }

    public struct XtmSettingsItemFields
    {
      public static readonly string Field_ExcludedFieldsId = "{3F7EA931-5932-4AC9-A04C-341008520FFA}";
      public static readonly string Field_BaseSiteUrlId = "{F8198642-8F33-4CC4-ACD8-0CC3905A480D}";
      public static readonly string Field_DebugModeId = "{4309F832-E30C-4333-9D0E-F31F14575CAB}";
      public static readonly string Field_InitialWorkflowState = "{5F337767-DCAA-4FED-A761-A48E6CB372B3}";
    }

    public XtmSettingsItem()
    {
      Database masteDb = ScConstants.SitecoreDatabases.MasterDb;
      Item settingsItem = masteDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsItem);
      BaseSiteUrl = settingsItem[XtmSettingsItemFields.Field_BaseSiteUrlId];
      HomeItem = masteDb.GetItem(settingsItem["{12078255-B2F0-4A71-822E-FDA1A75BC941}"]);
      ExcludedFieldsIds = settingsItem[XtmSettingsItemFields.Field_ExcludedFieldsId];
      CheckboxField DebugModeField = settingsItem.Fields[XtmSettingsItemFields.Field_DebugModeId];
      if (DebugModeField.Checked)
      {
        IsInDebugMode = true;
      }
      else
      {
        IsInDebugMode = false;
      }
      string initialWorkflowState = settingsItem[XtmSettingsItemFields.Field_InitialWorkflowState];
      if (initialWorkflowState.Equals(""))
      {
        InitialWorfklowStateId = ScConstants.SitecoreWorkflowIDs.XtmWorkflowStateAwaiting;        
      }
      else
      {
        InitialWorfklowStateId = initialWorkflowState;       
      }
      WorkflowId = masteDb.GetItem(InitialWorfklowStateId).ParentID.ToString();
    }
  }
}