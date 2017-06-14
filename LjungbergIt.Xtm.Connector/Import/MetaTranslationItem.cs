using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Import
{
  public class MetaTranslationItem
  {
    private struct XtmMetaTranslationTemplate
    {
      public static readonly TemplateID XtmMetaTranslationTemplateId = new TemplateID(new ID("{D4258A37-434A-4726-8720-70729A8227F9}"));
      public static readonly string TranslatedItemId = "{A8373522-3FE9-4BC4-AE72-B6C7AAD50A8F}";
      public static readonly string TranslatedFrom = "{FB2F09FC-30E4-46F8-8518-FE6325794848}";
      public static readonly string TranslatedTo = "{53DEBF78-3A46-45F2-84BE-077B933EBA86}";
      public static readonly string VersionNumber = "{662F3E94-A750-4678-BD69-4C082DE836F7}";
      public static readonly string ImportedDate = "{1C617837-C15C-4FA2-8D1E-3C72E75AB399}";
    }
  }
}