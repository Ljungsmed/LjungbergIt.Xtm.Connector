using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;

namespace LjungbergIt.Xtm.Connector.Import
{
    class GetLanguageQueueFolder
    {
        public Item GetFolder(string folderName)
        {
            Item returnItem = null;
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
                returnItem = CreateFolder(queueFolder, folderName);
            }

            return returnItem;
        }
        private Item CreateFolder(Item queueFolderItem, string folderName)
        {
            Item langFolderItem = null;
            using (new SecurityDisabler())
            {
                langFolderItem = queueFolderItem.Add(folderName, ScConstants.SitecoreTemplates.TranslationQueueLanguageFolderTemplate);
                langFolderItem.Editing.BeginEdit();
                langFolderItem[ScConstants.SitecoreFieldNames.TranslateTo] = folderName;
                langFolderItem.Editing.EndEdit();
            }

            return langFolderItem;
        }
    }
}
