using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Webservice;
using System.Collections.Generic;


namespace LjungbergIt.Xtm.Connector.Helpers
{
  public class StartTranslation
  {
    public ReturnMessage SendFilesToXtm(List<XtmTranslationFile> translationFiles, string fileName, TranslationProperties translationProperties)
    {
      ReturnMessage returnMessage = new ReturnMessage();
      XtmSettingsItem xtmSettingsItem = new XtmSettingsItem();

      if (!xtmSettingsItem.IsInDebugMode)
      {
        XtmCreateProject xtmCreateProject = new XtmCreateProject();

        LoginProperties login = new LoginProperties();
        XtmWebserviceProperties xtmWebserviceProperties = new XtmWebserviceProperties();

        //TODO move Translation Properties, Login Properties and XtmWebservice Properties to Separate project or Webservice Project and use inheritance to make super prop class
        List<string> result = xtmCreateProject.Create(translationFiles, fileName, translationProperties.SourceLanguage, translationProperties.TargetLanguages, translationProperties.XtmTemplate, login.ScClient, login.ScUserId, login.ScPassword, login.ScCustomer, xtmWebserviceProperties.WebserviceEndpoint, xtmWebserviceProperties.IsHttps, xtmWebserviceProperties.callBackUrl, translationProperties.DueDate);

        if (result[0].Equals("True"))
        {
          Project project = new Project();
          project.CreateProgressItem(result[1]);
          returnMessage.Success = true;
          returnMessage.Message = "Project with id " + result[1] + " successfully created in Xtm";
        }
        else
        {
          returnMessage.Success = false;
          returnMessage.Message = result[1];
        }
      }
      else
      {
        returnMessage.Success = false;
        returnMessage.Message = "No Projects created in XTM as the XTM Connector module is in Debug mode";
      }
      return returnMessage;
    }
  }
}
