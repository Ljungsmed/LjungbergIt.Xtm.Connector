using LjungbergIt.Xtm.Webservice.XtmServiceReference;
using System;
using System.Collections.Generic;

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
    public bool WorkflowFinished { get; set; }
    public string ProjectStatus { get; set; }
    public string Client { get; set; }
    public long UserId { get; set; }
    public string Password { get; set; }
    public bool ProjectError { get; set; }
    public string ProjectErrorMessage { get; set; }

    public XtmProject GetXtmProject(long projectId, XtmProject loginProject, string webServiceEndPoint, bool https)
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
      ProjectManagerMTOMWebServiceClient client = access.GetServiceClient(webServiceEndPoint, https);

      XtmProject xtmProject = new XtmProject();

      xtmProject.ProjectId = projectId;

      try
      {
        xtmProjectDetailsResponseAPI[] projectResponses = client.findProject(login, filter, null);
        //Assuming there is always only one project in the response
        xtmProject.ProjectError = false;
        xtmProject.ProjectName = projectResponses[0].name;
        xtmProject.Customer = projectResponses[0].customer.id;
        xtmProject.SourceLanguage = projectResponses[0].sourceLanguage.ToString();
        xtmProject.TargetLanguage = projectResponses[0].targetLanguages[0].ToString();
        xtmProject.CreatedDate = projectResponses[0].createDate;
        xtmProject.DueDate = projectResponses[0].dueDate;
        xtmProject.WorkflowStatus = projectResponses[0].status.ToString();
        if (projectResponses[0].status.Equals(xtmProjectStatusEnum.FINISHED))
        {
          WorkflowFinished = true;
        }
        else
        {
          WorkflowFinished = false;
        }
        xtmProject.ProjectStatus = projectResponses[0].activity.ToString();

      }
      catch (Exception e)
      {
        xtmProject.ProjectError = true;
        xtmProject.ProjectErrorMessage = e.Message;
        xtmProject.ProjectName = "Error finding project in XTM";

      }
      return xtmProject;
    }

    public List<XtmProject> GetProjects(List<long> projectIds, string client, long userId, string password, string webServiceEndPoint, bool https)
    {
      List<XtmProject> xtmProjects = new List<XtmProject>();
      XtmProject loginProject = new XtmProject { Client = client, UserId = userId, Password = password };

      foreach (long id in projectIds)
      {
        xtmProjects.Add(GetXtmProject(id, loginProject, webServiceEndPoint, https));
      }
      return xtmProjects;
    }
  }
}
