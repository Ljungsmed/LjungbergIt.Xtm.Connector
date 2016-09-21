using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Webservice
{
    public class XtmLanguage
    {
        public string LanguageName { get; set; }

        public List<XtmLanguage> GetXtmLanguages()
        {            
            List<XtmLanguage> languageList = new List<XtmLanguage>();

            foreach (XtmServiceReference.languageCODE langcode in Enum.GetValues(typeof(XtmServiceReference.languageCODE)))
            {
                languageList.Add( new XtmLanguage { LanguageName = langcode.ToString() } );
            }

            return languageList;
        }
    }
}
