using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;


namespace LjungbergIt.Xtm.Connector.Import
{
  public class MetaTranslationItem
  {

    Database master = ScConstants.SitecoreDatabases.MasterDb;
    private struct XtmMetaTranslationItem
    {
      public static readonly TemplateID TemplateId = new TemplateID(new ID("{29EECAA8-C54B-438A-BD0B-D04C59418F60}"));
      public static readonly string Field_ItemId = "{9E183F2F-56F1-40E7-9C39-5A8130B61B40}";
    }

    public Item GetMetaTranslationItem(Item newVersionItem, bool createNew)
    {
      //Find MetaTranslationItem, if it does not exist, create it
      //using search
      //Item MetaTranslationItem = master.GetItem(newVersionItem.ID);
      //if (MetaTranslationItem != null)
      //{
      //  return MetaTranslationItem;
      //}
      //else
      //{
        Item TranslationDataFolderItem = master.GetItem(ScConstants.SitecoreIDs.TranslationDataFolder);        
        List<UpdateItem> fieldObjects = new List<UpdateItem>();
        fieldObjects.Add(new UpdateItem { FieldIdOrName = XtmMetaTranslationItem.Field_ItemId, FieldValue = newVersionItem.ID.ToString() });
        UpdateItem updateItem = new UpdateItem();
        Item newMetaTranslationItem = updateItem.CreateItem(newVersionItem.Name + "_metatranslationItem", TranslationDataFolderItem, XtmMetaTranslationItem.TemplateId, fieldObjects);
        return newMetaTranslationItem;
      //}
    }
  }
}