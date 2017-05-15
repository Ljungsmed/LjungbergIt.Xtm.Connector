using LjungbergIt.Xtm.Connector.Export;
using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data.Items;
using Sitecore.Pipelines.GetContentEditorWarnings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjungbergIt.Xtm.Connector.MetaData.Pipelines.GetContentEditorWarnings
{
  public class InTranslation
  {
    public void Process(GetContentEditorWarningsArgs args)
    {
      XtmBaseTemplate xtmBaseTemplate = new XtmBaseTemplate(args.Item);
      if (xtmBaseTemplate.HasXtmBaseTemplate)
      {
        if (xtmBaseTemplate.InTranslation)
        {
          string labelId = "{79221E1E-725B-4D07-B253-34C3D3B5A1F8}";
          Item labelItem = ScConstants.SitecoreDatabases.MasterDb.GetItem(labelId);
          Label label = new Label(labelItem);
          args.Add(label.LabelTitle, label.LabelText);
        }
      }
      
      
    }
  }
}