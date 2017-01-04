using LjungbergIt.Xtm.Connector.AddForTranslation;
using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Webservice;
using System.Collections.Generic;


namespace LjungbergIt.Xtm.Connector.Export
{
    public class StartTranslation
    {
        public string SendFilesToXtm(List<XtmTranslationFile> translationFiles, string fileName, TranslationProperties translationProperties)
        {
            XtmCreateProject xtmCreateProject = new XtmCreateProject();
            
            LoginProperties login = new LoginProperties();
            XtmWebserviceProperties xtmWebserviceProperties = new XtmWebserviceProperties();

            string returnResult = "";
                  
            List<string> result = xtmCreateProject.Create(translationFiles, fileName, translationProperties.SourceLanguage, translationProperties.TargetLanguage, translationProperties.XtmTemplate, login.ScClient, login.ScUserId, login.ScPassword, login.ScCustomer, xtmWebserviceProperties.WebserviceEndpoint, xtmWebserviceProperties.IsHttps, xtmWebserviceProperties.callBackUrl, login.ScIntegrationKey );

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
