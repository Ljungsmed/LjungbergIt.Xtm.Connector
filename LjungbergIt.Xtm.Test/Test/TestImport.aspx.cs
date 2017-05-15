using LjungbergIt.Xtm.Webservice;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO.Compression;
using System.Xml;

namespace LjungbergIt.Xtm.Test.Test
{
  public partial class TestImport : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      //TEST id 1353198 has one translation file, but xtmJobFileMTOMResponseAPI[] jobs = downloadProject.project.jobs; returns 4 jobs, one where fileMTOM is null 
      //createFilesFromBytes();
      //string text = GetBytesFromProject(1353198);
      litInfo.Text = "DONE: ";
    }

    protected void btnCheckProjectId_click(object sender, EventArgs e)
    {
      string stringProjectId = txtboxProjectId.Text;

      long projectId = long.Parse(stringProjectId);

      createFilesFromBytes(projectId);
       
    }

    private List<byte[]> GetBytesFromProject(long projectId)
    {
      StringBuilder sb = new StringBuilder();
      TestLoginProps login = new TestLoginProps();

      XtmHandleTranslatedContent handleTranslatedContent = new XtmHandleTranslatedContent();
      sb.Append("test");
      List<byte[]> byteArray = handleTranslatedContent.GetFileInBytes(projectId, login.XtmClient, login.UserId, login.Password, login.WebServiceEndPoint, login.Https);
      //List<byte[]> byteArray1 = handleTranslatedContent.GetFileInBytes(1593980, login.XtmClient, login.UserId, login.Password, login.WebServiceEndPoint, login.Https);

      sb.Append(byteArray.Count.ToString());

      XtmProject xtmProject = new XtmProject();
      XtmProject loginProject = new XtmProject { Client = login.XtmClient, UserId = login.UserId, Password = login.Password };
      xtmProject = xtmProject.GetXtmProject( projectId, loginProject, login.WebServiceEndPoint, login.Https);
      litInfo.Text = xtmProject.ProjectStatus;

      return byteArray;
    }

    private void createFilesFromBytes(long projectId)
    {
      List<byte[]> bytesList = GetBytesFromProject(projectId);

      List<XmlDocument> xmlDocuments = new List<XmlDocument>();

      string filePath = System.Web.HttpContext.Current.Server.MapPath("~\\test\\");
      int count = 1;

      foreach (byte[] bytes in bytesList)
      {
        string zipFileName = filePath + "file" + count.ToString() + ".zip";
        string xmlFileName = filePath + "file" + count.ToString() + ".xml";

        File.WriteAllBytes(zipFileName, bytes);
        using (ZipArchive archive = ZipFile.OpenRead(zipFileName))
        {
          IReadOnlyCollection<ZipArchiveEntry> zipEntries = archive.Entries;
          foreach (ZipArchiveEntry entry in zipEntries)
          {
            XmlDocument xmlDoc = new XmlDocument();
            entry.ExtractToFile(xmlFileName, true);
            xmlDoc.Load(xmlFileName);
            xmlDocuments.Add(xmlDoc);
          }
        }
        //File.Delete(zipFileName);
        //File.Delete(xmlFileName);
        count++;
      }
    }
  }
}