using LjungbergIt.Xtm.Webservice.XtmServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Webservice
{
    public class XtmProject
    {
        public long projectId { get; set; }
        public string projectName { get; set; }
        public string customer { get; set; }
        public string sourceLanguage { get; set; }
        public string targetLanguage { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime dueDate { get; set; }
        public string workflowStatus { get; set; }

        public List<XtmProject> GetProjectProperties(List<long> projectIds)
        {
            List<XtmProject> xtmProjects = new List<XtmProject>();

            foreach (long id in projectIds)
            {
                xtmProjectDetailsResponseAPI project = GetXtmProject(id);

                XtmProject xtmProject = new XtmProject();
                
                xtmProject.projectId = id;
                xtmProject.projectName = project.name;
                xtmProject.customer = project.customer.name;
                xtmProject.sourceLanguage = project.sourceLanguage.ToString();
                xtmProject.targetLanguage = project.targetLanguages[0].ToString();
                xtmProject.createdDate = project.createDate;
                xtmProject.dueDate = project.dueDate;
                xtmProject.workflowStatus = project.status.ToString();

                xtmProjects.Add(xtmProject);
            }

            return xtmProjects;
        }

        public xtmProjectDetailsResponseAPI GetXtmProject(long projectId)
        {
            XtmWebserviceAccess xtmAccess = new XtmWebserviceAccess();
            loginAPI login = xtmAccess.Login();

            xtmProjectDescriptorAPI projectDescriptor = new xtmProjectDescriptorAPI();
            projectDescriptor.id = projectId;
            projectDescriptor.idSpecified = true;

            xtmProjectDescriptorAPI[] projects = new xtmProjectDescriptorAPI[] { projectDescriptor };
            xtmFilterProjectAPI filter = new xtmFilterProjectAPI();
            filter.projects = projects;

            ProjectManagerMTOMWebServiceClient client = new ProjectManagerMTOMWebServiceClient();

            xtmProjectDetailsResponseAPI[] projectResponses = client.findProject(login, filter, null);
            return projectResponses[0]; //Assuming there is always only one project in the response
        }

        public bool IsTranslationFinished(long projectId)
        {
            bool result = false;
   
            xtmProjectDetailsResponseAPI project = GetXtmProject(projectId);
            if (project.status.Equals(xtmProjectStatusEnum.FINISHED))
            {
                result = true;
            }

            return result;
        }
    }
}
