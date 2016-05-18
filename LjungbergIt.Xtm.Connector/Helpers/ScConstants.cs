using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Connector.Helpers
{
    class ScConstants
    {
        public struct SitecoreIDs
        {
            public static readonly string TranslationQueueFolder = "{2A38CC02-5466-4A5E-95AE-E16E5D06D1EC}";
            public static readonly string TranslationQueueArchiveFolder = "{0432E519-9A62-4D03-A0BB-7BFB83E43646}";
            public static readonly string TranslationInProgressFolder = "{D400D345-F302-4EAD-8B16-673094960139}";
            public static readonly string XtmSettingsItem = "{16952019-98B9-43EB-B1AB-18CD6663BE28}";

        }

        public struct SitecoreDatabases
        {
            public static readonly Database MasterDb = Database.GetDatabase("master");
            public static readonly Database WebDb = Database.GetDatabase("web");
            public static readonly Database ContextDb = Sitecore.Context.Database;
        }

        public struct SitecoreStandardFieldNames
        {
            //public static readonly string Workflow = "__workflow";
            //public static readonly string WorkflowState = "__workflow state";

        }

        public struct SitecoreTemplates
        {
            //public static readonly TemplateID TemplateName = new TemplateID(new ID("Id of the template"));
            public static readonly TemplateID TranslationQueueItemTemplate = new TemplateID(new ID("{C4A16DCB-2659-42D6-A641-89A83F34F358}"));
            public static readonly TemplateID TranslationQueueLanguageFolderTemplate = new TemplateID(new ID("{83825651-C491-4B03-9229-27A4954B38A9}"));
            public static readonly TemplateID TranslationInProgressTemplate = new TemplateID(new ID("{2D0F6780-6E62-428D-A52B-B560BDE0C8DC}"));
        }

        public struct SitecoreFieldNames
        {
            //public static readonly string MainTitle = "main title";
            public static readonly string TranslationQueueItem_ItemId = "ItemId";
            public static readonly string TranslationQueueItem_MasterLanguage = "MasterLanguage";
            public static readonly string TranslationQueueItem_TranslateTo = "TranslateTo";
            public static readonly string TranslationQueueItem_Version = "Version";
            public static readonly string Settings_MasterLanguage = "DefaultMasterLanguage";
            public static readonly string SitecoreLanguageItem_Iso = "iso";
            public static readonly string TranslateTo = "Translate To";
        }

        public struct SitecoreFieldIds
        {
            public static readonly string XtmProjectId = "{1EDFBAF9-9A37-4419-8C51-58882398F041}";
        }    

        public struct Misc
        {
            //public static readonly string EmailVaidationString = "\\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,}\\b";
            public static readonly string translationFolderName = "XTMData";
            public static readonly string filesFortranslationFolderName = "TranslationFiles";
        }

        public struct XmlNodes
        {
            //public static readonly TemplateID TemplateName = new TemplateID(new ID("Id of the template"));
            public static readonly string XmlRoot = "XtmTranslation";
            public static readonly string XmlRootElement = "SitecoreItem";
            public static readonly string XmlElementField = "field";
            public static readonly string XmlAttributeId = "id";
            public static readonly string XmlAttributeLanguage = "language";
            public static readonly string XmlAttributeVersion = "version";
            public static readonly string XmlAttributeFieldName = "fieldName";
            public static readonly string XmlAttributeFieldType = "fieldType";

        }
    }
}
