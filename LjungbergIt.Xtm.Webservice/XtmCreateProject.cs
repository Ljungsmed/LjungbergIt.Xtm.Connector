using LjungbergIt.Xtm.Webservice.XtmServiceReference;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LjungbergIt.Xtm.Webservice
{
    public class XtmCreateProject
    {
        public List<string> Create(string filePath, string fileName, string sourceLanguage, string translationLanguage, string xtmTemplate, string xtmClient, long userId, string password, long customer) //TODO how to map Sitecore languages to XTM languageCODE.language ???
        {
            List<string> resultList = new List<string>();
            bool success = false;
            StringBuilder info = new StringBuilder();
            //info.Append("ERROR");
            XtmWebserviceAccess access = new XtmWebserviceAccess();

            XtmProject project = new XtmProject();
            project.Client = xtmClient;
            project.UserId = userId;
            project.Password = password;
            project.Customer = customer;
            project.TargetLanguage = translationLanguage;
            project.SourceLanguage = sourceLanguage;
            project.Template = xtmTemplate;

            //TODO error handling if file not found
            byte[] fileInBytes = File.ReadAllBytes(filePath);
            xtmFileMTOMAPI fileToTranslate = new xtmFileMTOMAPI();
            StringBuilder projectName = new StringBuilder();
            projectName.Append(DateTime.Now.ToString("yyyyMMdd_"));
            projectName.Append(fileName);

            loginAPI login = access.Login(project);

            xtmCustomerDescriptorAPI xtmCustomer = access.XtmCustomer(2473);

            xtmWorkflowDescriptorAPI workflow = new xtmWorkflowDescriptorAPI();
            workflow.workflow = xtmWORKFLOWS.TRANSLATE;
            workflow.workflowSpecified = true;

            string targetLanguage = translationLanguage.Replace("-", "_");
            languageCODE parsedLanguage = new languageCODE();

            bool languageCodeParsed = languageCODE.TryParse(targetLanguage, true, out parsedLanguage);

            if (languageCodeParsed)
            {
                languageCODE?[] targetLang = new languageCODE?[] { parsedLanguage };
                fileToTranslate.fileName = "translate.xml";
                fileToTranslate.fileMTOM = fileInBytes;
                xtmFileMTOMAPI[] filesToTranslate = new xtmFileMTOMAPI[] { fileToTranslate };

                xtmProjectMTOMAPI projectMTOM = new xtmProjectMTOMAPI();
                projectMTOM.name = projectName.ToString();
                projectMTOM.sourceLanguage = languageCODE.en_GB;
                projectMTOM.sourceLanguageSpecified = true;
                projectMTOM.targetLanguages = targetLang;
                projectMTOM.customer = xtmCustomer;
                //projectMTOM.dueDate //Optional
                //projectMTOM.domain //Optional
                projectMTOM.workflow = workflow;
                //projectMTOM.workflowForNonAnalyzableFiles //Optional
                //projectMTOM.externalDescriptor //Optional
                if (!project.Template.Equals(""))
                {
                    xtmTemplateExtendedDescriptorAPI xtmTemplateDescriptor = new xtmTemplateExtendedDescriptorAPI();
                    //xtmTemplateDescriptor.name = project.Template;
                    xtmTemplateDescriptor.id = 3514;
                    xtmTemplateDescriptor.idSpecified = true;
                    projectMTOM.template = xtmTemplateDescriptor;
                }
                //projectMTOM.template //Optional
                projectMTOM.translationFiles = filesToTranslate;

                xtmCreateProjectMTOMOptionsAPI projectOptions = new xtmCreateProjectMTOMOptionsAPI();

                
                try
                {
                    ProjectManagerMTOMWebServiceClient client = new ProjectManagerMTOMWebServiceClient();
                    xtmCreateProjectMTOMResponseAPI response = client.createProjectMTOM(login, projectMTOM, null);// projectOptions);
                    info.Clear();
                    success = true;
                    info.Append(response.project.projectDescriptor.id.ToString());
                    //info.Append(success.ToString());                    
                }
                catch (Exception e)
                {
                    info.Append("CreateProjectError: " + e.ToString());
                    
                    //return info.ToString();
                }
            }
            else
            {
                info.Append("TargetLanguageError: target language not correctly parsed, the target language was: ");
                info.Append(targetLanguage);
            }

            resultList.Add(success.ToString());
            resultList.Add(info.ToString());
            return resultList;
        }
    }
}
