using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Connector.Helpers
{
    public class XtmBaseTemplateProperties
    {
        public bool Translated { get; set; }
        public DateTime TranslatedDate { get; set; }
        public string AddedToTranslationBy { get; set; }
        public bool InTranslation { get; set; }

        public XtmBaseTemplateProperties GetXtmBaseTemplateProperties(Item currentItem)
        {
            XtmBaseTemplateProperties properties = new XtmBaseTemplateProperties();
            return properties;
        }
    }
}
