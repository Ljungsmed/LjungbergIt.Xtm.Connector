using Sitecore.Collections;
using Sitecore.Data.Items;

namespace LjungbergIt.Xtm.Connector.Helpers
{
    public class TranslationItemCollection
    {
        public Item MainTranslationItem { get; set; }
        public ItemList RelatedItemsList { get; set; }

        public TranslationItemCollection(Item mainTranslationItem, ItemList relatedTranslationItems)
        {
            MainTranslationItem = mainTranslationItem;
            if (relatedTranslationItems.Count != 0)
            {
                RelatedItemsList = AddRelatedItem(mainTranslationItem, relatedTranslationItems) ;
            }
        }

        private ItemList AddRelatedItem(Item mainTranslationItem, ItemList relatedTranslationItems)
        {
            ItemList relatedItems = new ItemList();
            foreach (Item relatedItem in relatedTranslationItems)
            {
                string itemId = mainTranslationItem[TranslationQueueItem.XtmTranslationQueueItem.Field_ItemId];
                string relatedItemId = relatedItem[TranslationQueueItem.XtmTranslationQueueItem.Field_RelatedItemId];
                if (itemId.Equals(relatedItemId))
                {
                    relatedItems.Add(relatedItem);
                }
            }
            return relatedItems;
        }
    }
}