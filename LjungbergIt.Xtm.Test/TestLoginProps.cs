namespace LjungbergIt.Xtm.Test
{
  public class TestLoginProps
  {
    public string XtmClient { get; set; }
    public long UserId { get; set; }
    public string Password { get; set; }
    public string WebServiceEndPoint { get; set; }
    public bool Https { get; set; }

    public TestLoginProps()
    {
      XtmClient = "DTCM";
      UserId = 2652;
      Password = "DTCM";
      WebServiceEndPoint = "https://www.xtm-cloud.com/project-manager-gui/services/v2/projectmanager/XTMWebService";
      Https = true;
    }
  }
}