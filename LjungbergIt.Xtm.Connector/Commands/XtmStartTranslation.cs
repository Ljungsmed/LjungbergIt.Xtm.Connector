using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Connector.Helpers;
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
      string filePath = HttpContext.Current.Server.MapPath("~\\" + ScConstants.Misc.translationFolderName + "\\" + ScConstants.Misc.filesFortranslationFolderName + "\\");
      ConvertToXml convert = new ConvertToXml();
      convert.Transform(filePath);
    }
  }
}
