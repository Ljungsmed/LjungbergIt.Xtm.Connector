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

        public bool Update(Item itemToUpdate, List<UpdateItem> updateProps)
        {
            if (itemToUpdate != null)
            {
                using (new SecurityDisabler())
                {
                    foreach (UpdateItem updateProp in updateProps)
                    {
                        if (updateProp.FieldIdOrName != "")
                        {
                            itemToUpdate.Editing.BeginEdit();
                            itemToUpdate[updateProp.FieldIdOrName] = updateProp.FieldValue;
                            itemToUpdate.Editing.EndEdit();
                        }                        
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
            Sitecore.Diagnostics.Log.Info("XTMConnector: Trying to add item with name: " + itemName, this);
            using (new SecurityDisabler())
            {
                Item createdItem = parentItem.Add(itemName, templateId);
                if (createdItem != null)
                {
                    Update(createdItem, updateProps);
                }
            }
            return true;
        }

        public string CheckItemName(string itemName)
        {
            itemName.Replace("{", "");
            itemName.Replace("}", "");

            return itemName;
        }
    }

    
}
