using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Connector.AddForTranslation
{
    public class TranslationProperties
    {
        public string ItemId { get; set; }
        public string SourceLanguage { get; set; }
        public List<string> TargetLanguages { get; set; }
        public string XtmTemplate { get; set; }
    }
}
