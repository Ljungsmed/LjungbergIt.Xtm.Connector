using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using System.Collections.Generic;


namespace LjungbergIt.Xtm.Connector.Helpers
{
    public class UpdateItem
    {
        public string FieldIdOrName { get; set; }
        public string FieldValue { get; set; }


        //TODO change to get Item instead of ItemID!
        public bool Update(string itemId, List<UpdateItem> updateProps)
        {
            Item itemToUpdate = ScConstants.SitecoreDatabases.MasterDb.GetItem(itemId);

            if (itemToUpdate != null)
            {
                using (new SecurityDisabler())
                {
                    foreach (UpdateItem updateProp in updateProps)
                    {
                        itemToUpdate.Editing.BeginEdit();
                        itemToUpdate[updateProp.FieldIdOrName] = updateProp.FieldValue;
                        itemToUpdate.Editing.EndEdit();
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool CreateItem(string itemName, Item parentItem, TemplateID templateId, List<UpdateItem> updateProps)
        {
            using (new SecurityDisabler())
            {
                Item createdItem = parentItem.Add(itemName, templateId);
                if (createdItem != null)
                {
                    Update(createdItem.ID.ToString(), updateProps);
                }
            }
            return true;
        }
    }

    
}
