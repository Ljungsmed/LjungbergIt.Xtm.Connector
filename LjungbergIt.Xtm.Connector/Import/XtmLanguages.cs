using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Webservice;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Connector.Import
{
    public class XtmLanguages
    {
        public void CreateAllXtmLanguages()
        {
            DeleteAllXtmLanguages();

            XtmLanguage xtmLanguage = new XtmLanguage();
            List<XtmLanguage> xtmLanguageList = xtmLanguage.GetXtmLanguages();
            Item languageFolderItem = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsLanguageFolder);
            UpdateItem updateItem = new UpdateItem();

            foreach (XtmLanguage language in xtmLanguageList)
            {
                List<UpdateItem> updateProps = new List<UpdateItem>();
                updateProps.Add (new UpdateItem { FieldIdOrName = ScConstants.XtmLanguageTemplate.LanguageName, FieldValue = language.LanguageName });

                updateItem.CreateItem(language.LanguageName, languageFolderItem, ScConstants.SitecoreTemplates.XtmLanguage, updateProps);
            }
        }

        public void DeleteAllXtmLanguages()
        {
            Item languageFolderItem = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsLanguageFolder);
            using (new SecurityDisabler())
            {
                languageFolderItem.DeleteChildren();
            }
        }
    }
}
