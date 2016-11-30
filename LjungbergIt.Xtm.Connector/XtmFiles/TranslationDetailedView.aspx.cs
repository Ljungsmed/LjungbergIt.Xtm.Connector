using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LjungbergIt.Xtm.Connector.XtmFiles
{
    public partial class TranslationDetailedView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Database masterDb = ScConstants.SitecoreDatabases.MasterDb;
            List<ScQueueItem> queueItemList = new List<ScQueueItem>();
            ItemList currentQueue = new ItemList();
            Item queueFolder = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueFolder);
            foreach (Item queueCategoryItem in queueFolder.GetChildren())
            {
                foreach (Item queueItem in queueCategoryItem.GetChildren())
                {
                    Item translateItem = masterDb.GetItem(queueItem[ScConstants.XtmTranslationQueueItemTemplate.ItemId]);
                    string xtmTemplateId = queueCategoryItem[ScConstants.SitecoreFieldIds.QueuFolderXtmTemplate];
                    string xtmTemplate = string.Empty;
                    if (xtmTemplateId != "")
                    {
                        xtmTemplate = masterDb.GetItem(xtmTemplateId).Name;
                    }

                    ScQueueItem scQueueItem = new ScQueueItem();
                    scQueueItem.SourceLanguage = queueCategoryItem[ScConstants.SitecoreFieldIds.QueuFolderSourceLanguage];
                    scQueueItem.TargetLanguage = queueCategoryItem[ScConstants.SitecoreFieldIds.QueuFolderTranslateTo];
                    scQueueItem.XtmTemplate = xtmTemplate;
                    scQueueItem.QueueItemName = translateItem.Name;
                    scQueueItem.QueueItemPath = translateItem.Paths.ContentPath;
                    scQueueItem.AddedBy = queueItem[ScConstants.XtmTranslationQueueItemTemplate.AddedBy];

                    queueItemList.Add(scQueueItem);
                }
            }
            rptTranslationQueue.DataSource = queueItemList;
            rptTranslationQueue.DataBind();
        }
    }
}