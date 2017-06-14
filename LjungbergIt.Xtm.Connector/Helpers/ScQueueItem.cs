using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Connector.LanguageHandling
{
    public class ScQueueItem
    {
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; } //TODO delete?? Target language is on projectfolder, not individual items
        public string XtmTemplate { get; set; }

        public string QueueItemName { get; set; }
        public string QueueItemPath { get; set; }
        public string AddedBy { get; set; }
    }
}
