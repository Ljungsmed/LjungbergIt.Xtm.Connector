using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LjungbergIt.Xtm.Webservice.XtmServiceReference;
using System.Xml;
using System.IO;
using System.IO.Compression;

namespace LjungbergIt.Xtm.Webservice
{
    public class XtmHandleTranslatedContent
    {

        XtmWebserviceAccess xtmAccess = new XtmWebserviceAccess();

        public List<byte[]> GetFileInBytes(long projectID)
        {
            StringBuilder result = new StringBuilder();
            loginAPI login = xtmAccess.Login();

            xtmProjectDescriptorAPI projectDescriptor = new xtmProjectDescriptorAPI();
            projectDescriptor.id = projectID;
            projectDescriptor.idSpecified = true;

            ProjectManagerMTOMWebServiceClient client = new ProjectManagerMTOMWebServiceClient();

            xtmDownloadProjectMTOMResponseAPI downloadProject = client.downloadProjectMTOM(login, projectDescriptor, null);

            xtmJobFileMTOMResponseAPI[] jobs = downloadProject.project.jobs;

            List<byte[]> byteList = new List<byte[]>();

            foreach (xtmJobFileMTOMResponseAPI job in jobs)
            {
                byteList.Add(job.fileMTOM);
            }

            return byteList;
        }
        

    }
}

