using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Webservice;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Export
{
    public class StartTranslation
    {
        public string SendFilesToXtm()
        {
            XtmCreateProject create = new XtmCreateProject();
            Project project = new Project();
            StringBuilder info = new StringBuilder();
            string translationFolderName = ScConstants.Misc.translationFolderName;
            string filesForTranslationFolder = ScConstants.Misc.filesFortranslationFolderName;
            string translationFolder = HttpContext.Current.Server.MapPath("~\\" + translationFolderName + "\\" + filesForTranslationFolder + "\\");
            string[] FilesToTranslate = Directory.GetFiles(translationFolder);
            int count = 0;

            foreach (string file in FilesToTranslate)
            {
                int index = file.IndexOf(filesForTranslationFolder) + filesForTranslationFolder.Count() + 1;
                string targetLanguage = file.Remove(0, index);
                targetLanguage = targetLanguage.Replace(".xml", "");
                count++;
                info.Append("File ");
                info.Append(count);
                info.Append(" : Message from Webservice: ");
                string result = create.Create(file, targetLanguage);

                //TODO if result is not project id, handle it!
                project.CreateProgressItem(result);

                info.Append(result);
                info.Append(" END ");
                //File.Delete(file);
            }

            return info.ToString();
        }
    }
}
