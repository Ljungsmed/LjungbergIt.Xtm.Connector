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
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public long Customer { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string Template { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public string WorkflowStatus { get; set; }
        public string Client { get; set; }
        public long UserId { get; set; }
        public string Password { get; set; }

        public List<XtmProject> GetProjectProperties(List<long> projectIds, string client, long userId, string password, string webServiceEndPoint)
        {
            List<XtmProject> xtmProjects = new List<XtmProject>();
            XtmProject loginProject = new XtmProject { Client = client, UserId = userId, Password = password };

            foreach (long id in projectIds)
            {
                xtmProjectDetailsResponseAPI project = GetXtmProject(id, loginProject, webServiceEndPoint);

                XtmProject xtmProject = new XtmProject();
                
                xtmProject.ProjectId = id;
                xtmProject.ProjectName = project.name;
                xtmProject.Customer = project.customer.id;
                xtmProject.SourceLanguage = project.sourceLanguage.ToString();
                xtmProject.TargetLanguage = project.targetLanguages[0].ToString();
                xtmProject.CreatedDate = project.createDate;
                xtmProject.DueDate = project.dueDate;
                xtmProject.WorkflowStatus = project.status.ToString();

                xtmProjects.Add(xtmProject);
            }

            return xtmProjects;
        }

        public xtmProjectDetailsResponseAPI GetXtmProject(long projectId, XtmProject loginProject, string webServiceEndPoint)
        {
            XtmWebserviceAccess xtmAccess = new XtmWebserviceAccess();
            loginAPI login = xtmAccess.Login(loginProject);

            xtmProjectDescriptorAPI projectDescriptor = new xtmProjectDescriptorAPI();
            projectDescriptor.id = projectId;
            projectDescriptor.idSpecified = true;

            xtmProjectDescriptorAPI[] projects = new xtmProjectDescriptorAPI[] { projectDescriptor };
            xtmFilterProjectAPI filter = new xtmFilterProjectAPI();
            filter.projects = projects;

            XtmWebserviceAccess access = new XtmWebserviceAccess();
            ProjectManagerMTOMWebServiceClient client = access.GetServiceClient(webServiceEndPoint);
            //ProjectManagerMTOMWebServiceClient client = new ProjectManagerMTOMWebServiceClient();

            xtmProjectDetailsResponseAPI[] projectResponses = client.findProject(login, filter, null);
            return projectResponses[0]; //Assuming there is always only one project in the response
        }

        public bool IsTranslationFinished(long projectId, string client, long userId, string password, string webServiceEndPoint)
        {
            bool result = false;
            XtmProject loginProject = new XtmProject { Client = client, UserId = userId, Password = password };

            xtmProjectDetailsResponseAPI project = GetXtmProject(projectId, loginProject, webServiceEndPoint);
            if (project.status.Equals(xtmProjectStatusEnum.FINISHED))
            {
                result = true;
            }

            return result;
        }
    }
}
