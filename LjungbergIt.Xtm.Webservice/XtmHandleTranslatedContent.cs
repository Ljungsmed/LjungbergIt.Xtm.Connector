using System.Collections.Generic;
using System.Text;
using LjungbergIt.Xtm.Webservice.XtmServiceReference;


namespace LjungbergIt.Xtm.Webservice
{
    public class XtmHandleTranslatedContent
    {

        XtmWebserviceAccess xtmAccess = new XtmWebserviceAccess();

        public List<byte[]> GetFileInBytes(long projectID, string xtmClient, long userId, string password, string webServiceEndPoint, bool https)
        {
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

            if (jobs != null)
            {
                foreach (xtmJobFileMTOMResponseAPI job in jobs)
                {
                    byteList.Add(job.fileMTOM);
                }
            }

            return byteList;
        }
        

    }
}

