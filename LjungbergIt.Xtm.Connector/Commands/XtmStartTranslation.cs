using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Connector.Export;
using LjungbergIt.Xtm.Webservice;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Commands
{
    public class XtmStartTranslation
    {
        public void start()
        {
            ConvertToXml convert = new ConvertToXml();
            convert.Transform();
        }
    }
}
