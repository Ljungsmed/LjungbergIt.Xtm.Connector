using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.Collections;
using System.Collections.Generic;
using LjungbergIt.Xtm.Connector.Export;

namespace LjungbergIt.Xtm.Connector.Helpers
{
  public class XtmSettingsItem
  {
    public Item SettingsItem { get; set; }
    public string BaseSiteUrl { get; set; }
    //public Item HomeItem { get; set; }
    public bool IsInDebugMode { get; set; }
    public string ExcludedFieldsIds { get; set; }
    public List<string> ExcludedFieldTypesList { get; set; }
    public Item[] ExcludedFieldItems { get; set; }
    public string InitialWorfklowStateId { get; set; }
    public string WorkflowId { get; set; }

    public struct XtmSettingsItemFields
    {
      public static readonly string Field_ExcludedFieldsId = "{3F7EA931-5932-4AC9-A04C-341008520FFA}";
      public static readonly string Field_BaseSiteUrlId = "{F8198642-8F33-4CC4-ACD8-0CC3905A480D}";
      public static readonly string Field_DebugModeId = "{4309F832-E30C-4333-9D0E-F31F14575CAB}";
      public static readonly string Field_InitialWorkflowState = "{5F337767-DCAA-4FED-A761-A48E6CB372B3}";
      public static readonly string Field_ExcludedFieldTypesId = "{B9F9C621-FCC0-401F-86B6-37B2F83AE4F4}";
    }

    public XtmSettingsItem()
    {
      Database masteDb = ScConstants.SitecoreDatabases.MasterDb;
      SettingsItem = masteDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsItem);
      BaseSiteUrl = SettingsItem[XtmSettingsItemFields.Field_BaseSiteUrlId];
      //HomeItem = masteDb.GetItem(settingsItem["{12078255-B2F0-4A71-822E-FDA1A75BC941}"]);
      ExcludedFieldsIds = SettingsItem[XtmSettingsItemFields.Field_ExcludedFieldsId];
      ExcludedFieldTypesList = new List<string>();
      CheckboxField DebugModeField = SettingsItem.Fields[XtmSettingsItemFields.Field_DebugModeId];
      if (DebugModeField.Checked)
      {
        IsInDebugMode = true;
      }
      else
      {
        IsInDebugMode = false;
      }
      string initialWorkflowState = SettingsItem[XtmSettingsItemFields.Field_InitialWorkflowState];
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

    public Item[] GetExcludedFieldItems()
    {
      MultilistField ExcludedItemFields = SettingsItem.Fields[XtmSettingsItemFields.Field_ExcludedFieldsId];
      //ItemList test = ExcludedItemFields.GetItems();
      return ExcludedItemFields.GetItems();
    }
    public List<string> GetExcludedFieldTypes()
    {
      MultilistField ExcludedFieldTypesItems = SettingsItem.Fields[XtmSettingsItemFields.Field_ExcludedFieldTypesId];
      foreach (Item ExcludedFieldTypesItem in ExcludedFieldTypesItems.GetItems())
      {
        ExcludedFieldTypesList.Add(ExcludedFieldTypesItem[ExcludedFields.XtmFieldTypeFields.Field_FieldTypeName].ToLower());
      }
      return ExcludedFieldTypesList;
    }
  }
}