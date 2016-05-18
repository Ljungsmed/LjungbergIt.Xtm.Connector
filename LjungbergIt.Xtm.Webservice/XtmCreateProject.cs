using LjungbergIt.Xtm.Webservice.XtmServiceReference;
using System;
using System.IO;
using System.Text;

namespace LjungbergIt.Xtm.Webservice
{
    public class XtmCreateProject
    {
        public string Create(string filePath, string translationLanguage) //TODO how to map Sitecore languages to XTM languageCODE.language ???
        {
            string info = "failure";
            XtmWebserviceAccess access = new XtmWebserviceAccess();


            //TODO error handling if file not found
            byte[] fileInBytes = File.ReadAllBytes(filePath);
            xtmFileMTOMAPI fileToTranslate = new xtmFileMTOMAPI();
            StringBuilder projectName = new StringBuilder();
            projectName.Append(DateTime.Now.ToString("yyyyMMdd"));
            //projectName.Append("_TargetLanguage_");
            //projectName.Append(translationLanguage);
            loginAPI login = access.Login();

            xtmCustomerDescriptorAPI customer = access.XtmCustomer(2473);

            xtmWorkflowDescriptorAPI workflow = new xtmWorkflowDescriptorAPI();
            workflow.workflow = xtmWORKFLOWS.TRANSLATE;
            workflow.workflowSpecified = true;

            languageCODE parsedLanguage = new languageCODE();

            bool languageCodeParsed = languageCODE.TryParse("fr_FR", true, out parsedLanguage);

            languageCODE?[] targetLang = new languageCODE?[] { parsedLanguage };
            fileToTranslate.fileName = "translate.xml";
            fileToTranslate.fileMTOM = fileInBytes;
            xtmFileMTOMAPI[] filesToTranslate = new xtmFileMTOMAPI[] { fileToTranslate };

            xtmProjectMTOMAPI projectMTOM = new xtmProjectMTOMAPI();
            projectMTOM.name = projectName.ToString();
            projectMTOM.sourceLanguage = languageCODE.en_GB;
            projectMTOM.sourceLanguageSpecified = true;
            projectMTOM.targetLanguages = targetLang;
            projectMTOM.customer = customer;
            //projectMTOM.dueDate //Optional
            //projectMTOM.domain //Optional
            projectMTOM.workflow = workflow;
            //projectMTOM.workflowForNonAnalyzableFiles //Optional
            //projectMTOM.externalDescriptor //Optional
            //projectMTOM.template //Optional
            projectMTOM.translationFiles = filesToTranslate;

            xtmCreateProjectMTOMOptionsAPI projectOptions = new xtmCreateProjectMTOMOptionsAPI();

            //try
            //{
            //    ProjectManagerMTOMWebServiceClient client = new ProjectManagerMTOMWebServiceClient();
            //    xtmCreateProjectMTOMResponseAPI response = client.createProjectMTOM(login, projectMTOM, null);// projectOptions);
            //    info = response.project.projectDescriptor.id.ToString();
            //}
            //catch (Exception e)
            //{
            //    info = "ERROR: " + e.ToString();
            //    return info;
            //}

            return info;
        }
    }
}
