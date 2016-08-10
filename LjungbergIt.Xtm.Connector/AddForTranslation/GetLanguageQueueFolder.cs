﻿using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel;

namespace LjungbergIt.Xtm.Connector.AddForTranslation
{
    class GetLanguageQueueFolder
    {
        public Item GetFolder(string folderName, TranslationProperties translationProperties)
        {
            Item returnItem = null;

            ScUser scUser = new ScUser();
            User user = scUser.GetUser();
            using (new UserSwitcher(user))
            {
                Item queueFolder = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueFolder);
                foreach (Item folderItem in queueFolder.GetChildren())
                {
                    if (folderItem.Name.Equals(folderName))
                    {
                        returnItem = folderItem;
                    }
                }

                if (returnItem == null)
                {
                    returnItem = CreateFolder(queueFolder, folderName, translationProperties);
                }
            }

            return returnItem;
        }
        private Item CreateFolder(Item queueFolderItem, string folderName, TranslationProperties translationProperties)
        {
            Item langFolderItem = null;
            if (translationProperties.XtmTemplate.Equals("NONE"))
            {
                translationProperties.XtmTemplate = string.Empty;
            }

            ScUser scUser = new ScUser();
            User user = scUser.GetUser();
            using (new UserSwitcher(user))
            {
                langFolderItem = queueFolderItem.Add(folderName, ScConstants.SitecoreTemplates.TranslationQueueLanguageFolderTemplate);
                langFolderItem.Editing.BeginEdit();
                langFolderItem[ScConstants.SitecoreFieldIds.QueuFolderSourceLanguage] = translationProperties.SourceLanguage;
                langFolderItem[ScConstants.SitecoreFieldIds.QueuFolderTranslateTo] = translationProperties.TargetLanguage;
                langFolderItem[ScConstants.SitecoreFieldIds.QueuFolderXtmTemplate] = translationProperties.XtmTemplate;
                langFolderItem.Editing.EndEdit();
            }

            return langFolderItem;
        }
    }
}
