using System.Collections.Generic;
using System.Text;
using LjungbergIt.Xtm.Webservice.XtmServiceReference;


namespace LjungbergIt.Xtm.Webservice
{
  public class XtmHandleTranslatedContent
  { 
    public List<XtmJob> GetTranslatedJobs(long projectID, string xtmClient, long userId, string password, string webServiceEndPoint, bool https)
    {
      XtmWebserviceAccess xtmAccess = new XtmWebserviceAccess();
      XtmProject project = new XtmProject { Client = xtmClient, UserId = userId, Password = password };
      StringBuilder result = new StringBuilder();
      loginAPI login = xtmAccess.Login(project);

      xtmProjectDescriptorAPI projectDescriptor = new xtmProjectDescriptorAPI();
      projectDescriptor.id = projectID;
      projectDescriptor.idSpecified = true;

      ProjectManagerMTOMWebServiceClient client = xtmAccess.GetServiceClient(webServiceEndPoint, https);

      xtmDownloadProjectMTOMResponseAPI downloadProject = client.downloadProjectMTOM(login, projectDescriptor, null);

      xtmJobFileMTOMResponseAPI[] jobs = downloadProject.project.jobs;

      List<byte[]> byteList = new List<byte[]>();
      List<XtmJob> xtmJobList = new List<XtmJob>();

      if (jobs != null)
      {
        foreach (xtmJobFileMTOMResponseAPI job in jobs)
        {
          if (job.fileMTOM != null)
          {
            XtmJob xtmJob = new XtmJob()
            {
              TargetLanguage = job.targetLanguage.ToString(),
              TranslationFileInBytes = job.fileMTOM
            };
            xtmJobList.Add(xtmJob);
            byteList.Add(job.fileMTOM);
          }
        }
      }

      return xtmJobList;
    }


  }
}

