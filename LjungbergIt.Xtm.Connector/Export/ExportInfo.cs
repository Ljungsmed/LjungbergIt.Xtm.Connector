using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Helpers
{
  public class ExportInfo
  {
    public Item ExportInfoItem { get; set; }
    public string StatusField { get; set; }
    public string StartField { get; set; }
    public string EndField { get; set; }
    public string TotalTime { get; set; }
    public string InitiatedBy { get; set; }
    public string ErrorsField { get; set; }


    private static readonly string ExportInfoItemId = "{0711457F-84AA-4ACE-9680-6FE8795C6381}";
    private static readonly string StatusFieldId = "{B4DE218F-8D9D-4359-84C0-E13C57BF6121}";
    private static readonly string StartFieldId = "{1B9DFE65-4CE3-4118-B210-03ADE1957997}";
    private static readonly string EndFieldId = "{A2763D04-36E0-491B-904A-D6BEBC7B76A5}";
    private static readonly string TotalTimeFieldId = "{F8F11D6E-F489-429C-A0AA-1B46FD7A5953}";
    private static readonly string InitiatedByFieldId = "{3A751D28-222C-4529-B7C5-B942C842DEB2}";
    private static readonly string ErrorsFieldId = "{ABE52C6E-5AB7-4FD2-B801-C1C631611A5F}";
    public ExportInfo()
    {
      Database master = ScConstants.SitecoreDatabases.MasterDb;
      ExportInfoItem = master.GetItem(ExportInfoItemId);
      //StatusField = ExportInfoItem.Fields[StatusFieldId];
      StatusField = StatusFieldId;
      StartField = StartFieldId;
      EndField = EndFieldId;
      TotalTime = TotalTimeFieldId;
      InitiatedBy = InitiatedByFieldId;
      ErrorsField = ErrorsFieldId;
    }

    public void UpdateValueField(string valueToUpdate, string itemId, string fieldName, bool overwrite)
    {
      StringBuilder errorString = new StringBuilder();
      if (!overwrite)
      {
        errorString.Append(ExportInfoItem[ErrorsField]);        
      }
      errorString.Append("ERROR: ");
      errorString.Append(valueToUpdate);
      errorString.Append(" In field with name: ");
      errorString.Append(fieldName);
      errorString.Append(" from Item with id: ");
      errorString.Append(itemId);
      errorString.Append(";");

      //valueToUpdate = ExportInfoItem[StatusFieldId] + " " + valueToUpdate;
      List<UpdateItem> updateProps = new List<UpdateItem>();
      UpdateItem updateItem = new UpdateItem();
      updateItem.FieldIdOrName = ErrorsField;
      updateItem.FieldValue = errorString.ToString();
      updateProps.Add(updateItem);
      updateItem.Update(ExportInfoItem, updateProps);
    }
  }
}