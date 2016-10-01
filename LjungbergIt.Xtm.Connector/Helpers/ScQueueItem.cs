﻿using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Connector.Helpers
{
    public class ScQueueItem
    {
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string XtmTemplate { get; set; }
        public Item QueueItem { get; set; }
        public string AddedBy { get; set; }
    }
}