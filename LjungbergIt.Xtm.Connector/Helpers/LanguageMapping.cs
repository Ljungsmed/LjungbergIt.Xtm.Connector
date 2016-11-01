using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using System.Collections.Generic;

namespace LjungbergIt.Xtm.Connector.Helpers
{
    class LanguageMapping
    {
        public string LangName { get; set; }
        public string XtmLangName { get; set; }

        public List<LanguageMapping> LanguageList()
        {
            List<LanguageMapping> languageMapping = new List<LanguageMapping>();
            
            using (new SecurityDisabler())
            {
                Item languageMappingFolder = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsLanguageMappingFolder);

                foreach (Item languageMappingItem in languageMappingFolder.GetChildren())
                {
                    Item xtmLanguageItem = ScConstants.SitecoreDatabases.MasterDb.GetItem(languageMappingItem[ScConstants.SitecoreFieldIds.XtmLanguageName]);
                    languageMapping.Add(new LanguageMapping() { LangName = languageMappingItem[ScConstants.SitecoreFieldIds.SitecoreLanguageName], XtmLangName = xtmLanguageItem.Name });
                }
            }
            return languageMapping;
        }
    }
}
