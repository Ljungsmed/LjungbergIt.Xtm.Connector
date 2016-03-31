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
            
        }

        public struct SitecoreFieldNames
        {
            //public static readonly string MainTitle = "main title";
            public static readonly string TranslationQueueItem_ItemId = "ItemId";
            public static readonly string TranslationQueueItem_MasterLanguage = "MasterLanguage";
            public static readonly string TranslationQueueItem_TranslateTo = "TranslateTo";
            public static readonly string TranslationQueueItem_Version = "Version";
        }    

        public struct Misc
        {
            //public static readonly string EmailVaidationString = "\\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,}\\b";
        }
    }
}
