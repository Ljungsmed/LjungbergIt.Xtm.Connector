using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Pipelines.GetContentEditorWarnings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjungbergIt.Xtm.Connector.MetaData.Pipelines.GetContentEditorWarnings
{
  public class VersionTranslated
  {
    public void Process(GetContentEditorWarningsArgs args)
    {
      CheckboxField translatedField = args.Item.Fields[XtmBaseTemplate.Ids.TranslatedFieldId];
      if (translatedField != null)
      {
        if (translatedField.Checked)
        {
          string labelId = "{BABA9780-BCF4-4E87-A809-1F381A7D2A30}";
          Item labelItem = ScConstants.SitecoreDatabases.MasterDb.GetItem(labelId);
          DateField translationDate = args.Item.Fields[XtmBaseTemplate.Ids.TranslationDateFieldId];
          string translatedTFrom = args.Item[XtmBaseTemplate.Ids.TranslatedFromFieldId];
          Label label = new Label(labelItem);
          args.Add(label.LabelTitle, string.Format(label.LabelText, translationDate.DateTime.ToString(label.LabelFormat), translatedTFrom));
        }
      }
    }
  }
}