using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Import
{
  public class MetaTranslationVersion
  {
    private struct XtmMetaTranslationVersion
    {
      public static readonly TemplateID TemplateId = new TemplateID(new ID("{D4258A37-434A-4726-8720-70729A8227F9}"));
      public static readonly string Field_TranslatedItemId = "{A8373522-3FE9-4BC4-AE72-B6C7AAD50A8F}";
      public static readonly string Field_TranslatedFrom = "{FB2F09FC-30E4-46F8-8518-FE6325794848}";
      public static readonly string Field_TranslatedTo = "{53DEBF78-3A46-45F2-84BE-077B933EBA86}";
      public static readonly string Field_VersionNumber = "{662F3E94-A750-4678-BD69-4C082DE836F7}";
      public static readonly string Field_ImportedDate = "{1C617837-C15C-4FA2-8D1E-3C72E75AB399}";
      public static readonly string Field_XtmProjectId = "{FABCF8B2-67EF-4499-95A8-57D55E3C66CF}";
    }

    public Item CreateMetaTranslationVersion(Item newVersionItem, string sourceLanguage, string xtmProjectId)
    {
      string version = newVersionItem.Version.Number.ToString();
      string targetLanguage = newVersionItem.Language.Name;
      string itemName = newVersionItem.Name + "_" + version + "_" + targetLanguage;

      MetaTranslationItem metaTranslationItem = new MetaTranslationItem();
      Item newMetaTranslationItem = metaTranslationItem.GetMetaTranslationItem(newVersionItem, true);

      List<UpdateItem> fieldObjects = new List<UpdateItem>();
      fieldObjects.Add(new UpdateItem { FieldIdOrName = XtmMetaTranslationVersion.Field_TranslatedFrom, FieldValue = sourceLanguage });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = XtmMetaTranslationVersion.Field_TranslatedTo, FieldValue = targetLanguage });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = XtmMetaTranslationVersion.Field_VersionNumber, FieldValue = version });
      //TODO change to proper date
      fieldObjects.Add(new UpdateItem { FieldIdOrName = XtmMetaTranslationVersion.Field_ImportedDate, FieldValue = newVersionItem.Statistics.Updated.ToString() });
      fieldObjects.Add(new UpdateItem { FieldIdOrName = XtmMetaTranslationVersion.Field_XtmProjectId, FieldValue = xtmProjectId });

      UpdateItem updateItem = new UpdateItem();
      Item newMetaTranslationVersionItem = updateItem.CreateItem(itemName, newMetaTranslationItem, XtmMetaTranslationVersion.TemplateId, fieldObjects);

      return newMetaTranslationVersionItem;
    }

  }
}