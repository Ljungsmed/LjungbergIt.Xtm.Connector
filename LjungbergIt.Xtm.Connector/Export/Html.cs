using LjungbergIt.Xtm.Connector.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Export
{
    public class Html
    {
        public void GenerateHtml(string html)
        {
            string folder = ScConstants.Misc.translationFolderName + @"\" + ScConstants.Misc.filesFortranslationFolderName;

            string filePath = @"~\" + ScConstants.Misc.translationFolderName + @"\" + ScConstants.Misc.filesFortranslationFolderName + @"\" + "fileName" + ".html";
            string fullFilePath = HttpContext.Current.Server.MapPath(filePath);

            System.IO.File.WriteAllText(fullFilePath, html);
        }
    }
}