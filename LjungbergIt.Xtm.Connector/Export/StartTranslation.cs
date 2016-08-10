using LjungbergIt.Xtm.Connector.AddForTranslation;
using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Webservice;
using System.Collections.Generic;
using System.IO;

namespace LjungbergIt.Xtm.Connector.Export
{
    public class StartTranslation
    {
        public string SendFilesToXtm(string filePath, string fileName, TranslationProperties translationProperties)
        {
            XtmCreateProject xtmCreateProject = new XtmCreateProject();
            
            LoginProperties login = new LoginProperties();
            string returnResult = "";
                  
            List<string> result = xtmCreateProject.Create(filePath, fileName, translationProperties.SourceLanguage, translationProperties.TargetLanguage, translationProperties.XtmTemplate, login.ScClient, login.ScUserId, login.ScPassword, login.ScCustomer );

            if (result[0].Equals("True"))
            {
                Project project = new Project();
                project.CreateProgressItem(result[1]);
                returnResult = "success";
            }
            else
            {
                returnResult = result[1];
            }
           
            //File.Delete(filePath);            

            return returnResult;
        }
    }
}
