using LjungbergIt.Xtm.Webservice.XtmServiceReference;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LjungbergIt.Xtm.Webservice
{
    public class XtmWebserviceAccess
    {
        public string XtmCreateProject(string filePath, string translationLanguage) //TODO how to map Sitecore languages to XTM languageCODE.language ???
        {
            string info = "success";

            //TODO error handling if file not found
            byte[] fileInBytes = File.ReadAllBytes(filePath);
            xtmFileMTOMAPI fileToTranslate = new xtmFileMTOMAPI();
            StringBuilder projectName = new StringBuilder();
            projectName.Append(DateTime.Now.ToString("yyyyMMdd"));
            //projectName.Append("_TargetLanguage_");
            //projectName.Append(translationLanguage);
            loginAPI login = Login();

            xtmCustomerDescriptorAPI customer = XtmCustomer(2473);

            xtmWorkflowDescriptorAPI workflow = new xtmWorkflowDescriptorAPI();
            workflow.workflow = xtmWORKFLOWS.TRANSLATE;
            workflow.workflowSpecified = true;

            languageCODE?[] targetLang = new languageCODE?[] { languageCODE.fr_FR};            
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

            try
            {
                ProjectManagerMTOMWebServiceClient client = new ProjectManagerMTOMWebServiceClient();
                xtmCreateProjectMTOMResponseAPI response = client.createProjectMTOM(login, projectMTOM, null);// projectOptions);
                info = response.project.projectDescriptor.id.ToString();
            }
            catch (Exception e)
            {
                info = "ERROR: " + e.ToString();
                return info;
            }

            return info;           
        }

        public loginAPI Login()
        {
            loginAPI login = new loginAPI();
            login.client = "sitecore";
            login.userId = 2462;
            login.password = "sitecore.95";
            login.userIdSpecified = true;

            return login;
        }

        public xtmCustomerDescriptorAPI XtmCustomer(long customerId)
        {
            xtmCustomerDescriptorAPI customer = new xtmCustomerDescriptorAPI();
            customer.id = customerId; // 2462;
            customer.idSpecified = true;
            return customer;
        }
    }
}
