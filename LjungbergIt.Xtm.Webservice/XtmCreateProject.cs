using LjungbergIt.Xtm.Webservice.XtmServiceReference;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace LjungbergIt.Xtm.Webservice
{
  public class XtmCreateProject
  {
    public List<string> Create(List<XtmTranslationFile> translationFiles, string fileName, string sourceLanguage, List<string> translationLanguages, string xtmTemplate, string xtmClient, long userId, string password, long customer, string webServiceEndPoint, bool https, string callbackUrl)
    {
      List<string> resultList = new List<string>();
      bool success = false;
      StringBuilder info = new StringBuilder();
      XtmWebserviceAccess access = new XtmWebserviceAccess();

      XtmProject project = new XtmProject();
      project.Client = xtmClient;
      project.UserId = userId;
      project.Password = password;
      project.Customer = customer;
      //FIX
      //project.TargetLanguage = translationLanguage;
      project.SourceLanguage = sourceLanguage;
      project.Template = xtmTemplate;

      xtmFileMTOMAPI[] filesToTranslate = new xtmFileMTOMAPI[translationFiles.Count];
      int count = 0;
      foreach (XtmTranslationFile translationFile in translationFiles)
      {
        byte[] fileInBytes = File.ReadAllBytes(translationFile.FilePath);
        xtmFileMTOMAPI fileToTranslate = new xtmFileMTOMAPI();
        fileToTranslate.fileName = translationFile.FileName;
        fileToTranslate.fileMTOM = fileInBytes;
        filesToTranslate[count] = fileToTranslate;
        count++;
        File.Delete(translationFile.FilePath);
      }

      StringBuilder projectName = new StringBuilder();
      projectName.Append(fileName);

      loginAPI login = access.Login(project);

      xtmCustomerDescriptorAPI xtmCustomer = access.XtmCustomer(customer);

      //Convert the target language strings to Xtm langauges and add them to the list of target languages
      languageCODE?[] targetLang = new languageCODE?[translationLanguages.Count];
      for (int i = 0; i < translationLanguages.Count; i++)
      {
        languageCODE parsedLanguage = new languageCODE();
        bool languageCodeParsed = languageCODE.TryParse(translationLanguages[i], true, out parsedLanguage);
        targetLang[i] = parsedLanguage;
      }

      //TODO move to configuration
      string sitecoreSourceLanguage = sourceLanguage.Replace("-", "_");
      if (sitecoreSourceLanguage == "en")
      {
        sitecoreSourceLanguage = "en_GB";
      }
      languageCODE parsedSourceLanguage = new languageCODE();
      bool sourceLanguageCodeParsed = languageCODE.TryParse(sitecoreSourceLanguage, true, out parsedSourceLanguage);

      //Set the callback URL that XTM will call when the project is finished and include a JSON response in the POST
      xtmProjectCallbackAPI callBackAPI = new xtmProjectCallbackAPI();
      callBackAPI.projectFinishedCallback = callbackUrl;

      if (sourceLanguageCodeParsed)
      {
        xtmProjectMTOMAPI projectMTOM = new xtmProjectMTOMAPI();
        projectMTOM.name = projectName.ToString();
        projectMTOM.sourceLanguage = parsedSourceLanguage;
        projectMTOM.sourceLanguageSpecified = true;
        projectMTOM.targetLanguages = targetLang;
        projectMTOM.customer = xtmCustomer;
        projectMTOM.projectCallback = callBackAPI;
        projectMTOM.translationFiles = filesToTranslate;
        //projectMTOM.description = 

        if (!project.Template.Equals(""))
        {
          xtmTemplateExtendedDescriptorAPI xtmTemplateDescriptor = new xtmTemplateExtendedDescriptorAPI();
          //xtmTemplateDescriptor.name = project.Template;
          xtmTemplateDescriptor.id = long.Parse(project.Template);
          xtmTemplateDescriptor.idSpecified = true;
          projectMTOM.template = xtmTemplateDescriptor;
        }
        else
        {
          xtmWorkflowDescriptorAPI workflow = new xtmWorkflowDescriptorAPI();
          workflow.workflowSpecified = true;
          workflow.workflow = xtmWORKFLOWS.TRANSLATE;
          projectMTOM.workflow = workflow;
        }

        //xtmCreateProjectMTOMOptionsAPI projectOptions = new xtmCreateProjectMTOMOptionsAPI();

        try
        {
          ProjectManagerMTOMWebServiceClient client = access.GetServiceClient(webServiceEndPoint, https);
          xtmCreateProjectMTOMResponseAPI response = client.createProjectMTOM(login, projectMTOM, null);// projectOptions);
          //info.Clear();
          success = true;
          //return both name and id
          info.Append(response.project.projectDescriptor.id.ToString());

          XtmProjectFiles xtmUpdateProject = new XtmProjectFiles();

          foreach (XtmTranslationFile translationFile in translationFiles)
          {
            if (translationFile.HtmlFileAvailable)
            {
              xtmUploadProjectFileMTOMAPI previewFile = xtmUpdateProject.GetPreviewFile(translationFile.HtmlFilePath, translationFile.FileName);
              previewFile.projectDescriptor = response.project.projectDescriptor;
              client.uploadProjectFileMTOM(login, previewFile, null);

              //DEBUG comment the below line out to check the HTML files generated
              File.Delete(translationFile.HtmlFilePath);
            }
          }
        }
        catch (Exception e)
        {
          info.Append("CreateProjectError: " + e.ToString());
        }
      }
      else
      {
        info.Append("Source language error: source language not correctly parsed, the source language was: ");
        //info.Append(targetLanguages[0]);
      }

      resultList.Add(success.ToString());
      resultList.Add(info.ToString());
      return resultList;
    }
  }
}
