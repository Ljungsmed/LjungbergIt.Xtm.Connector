using LjungbergIt.Xtm.Connector.Export;
using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data.Items;
using Sitecore.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Export
{
  public class TranslationItem
  {
    public int SubItemLevel { get; set; }
    public Item sitecoreItem { get; set; }
    public bool Translatable { get; set; }
    public string HelpText { get; set; }
    public Item RelatesTo { get; set; }

    //string helpText = "Check the box to include the item for translation";
    //string helpTextNotAvailable = "This item cannot be added, as it is not configured for translation. Please notify your XTM administrator if this is a mistake";
    public List<TranslationItem> GetListOfSubItems(Item parent, List<TranslationItem> subItemsList, int level)
    {
      foreach (Item child in parent.GetChildren())
      {
        XtmBaseTemplate xtmBaseTemplate = new XtmBaseTemplate(child);
        string helpText = "Check the box to include the item for translation";
        int numberOfChildren = child.GetChildren().Count;
        //string helpText = "Check the box to include the item for translation";
        if (!xtmBaseTemplate.HasXtmBaseTemplate)
        {
          helpText = "This item cannot be added, as it is not configured for translation. Please notify your XTM administrator if this is a mistake";
        }        
        subItemsList.Add(new TranslationItem { sitecoreItem = child, SubItemLevel = level, Translatable=xtmBaseTemplate.HasXtmBaseTemplate, HelpText= helpText });
        if (numberOfChildren != 0) 
        {
          GetListOfSubItems(child, subItemsList, level+1);
        }
      }
      return subItemsList;
    }
    public List<TranslationItem> GetRelatedContentItems(Item item)
    {
      List<TranslationItem> relatedItems = new List<TranslationItem>();

      Item defaultDeviceItem = ScConstants.SitecoreDatabases.MasterDb.GetItem("{FE5D7FDF-89C0-4D99-9AA3-B5FBD009C9F3}");

      DeviceItem deviceItem = new DeviceItem(defaultDeviceItem);
      RenderingReference[] itemRenderings = item.Visualization.GetRenderings(defaultDeviceItem, true);

      foreach (RenderingReference itemRendering in itemRenderings)
      {
        string datasource = itemRendering.Settings.DataSource; // itemRendering.RenderingItem.DataSource;
        if (datasource != "")
        {
          Item relatedItem = ScConstants.SitecoreDatabases.MasterDb.GetItem(datasource);
          if (relatedItem != null)
          {
            XtmBaseTemplate xtmBaseTemplate = new XtmBaseTemplate(relatedItem);
            string helpText = "Check the box to include the item for translation";
            if (!xtmBaseTemplate.HasXtmBaseTemplate)
            {
              helpText = "This item cannot be added, as it is not configured for translation. Please notify your XTM administrator if this is a mistake";
            }
            TranslationItem subItem = new TranslationItem { sitecoreItem = relatedItem, Translatable = xtmBaseTemplate.HasXtmBaseTemplate, HelpText = helpText, RelatesTo = item };
            if (!relatedItems.Any(n => n.sitecoreItem.ID == relatedItem.ID))
            {
              relatedItems.Add(subItem);
            }
          }
        }
      }
      return relatedItems;
    }
  }
}