using LjungbergIt.Xtm.Webservice.XtmServiceReference;
using System.ServiceModel;

namespace LjungbergIt.Xtm.Webservice
{
  public class XtmWebserviceAccess
  {
    public loginAPI Login(XtmProject project)
    {
      loginAPI login = new loginAPI();
      login.client = project.Client;
      login.userId = project.UserId;
      login.password = project.Password;
      login.integrationKey = "765c8be4597c4c01a978ddff10626f42";
      login.userIdSpecified = true;

      return login;
    }

    public xtmCustomerDescriptorAPI XtmCustomer(long customerId)
    {
      xtmCustomerDescriptorAPI customer = new xtmCustomerDescriptorAPI();
      customer.id = customerId;
      customer.idSpecified = true;
      return customer;
    }

    public ProjectManagerMTOMWebServiceClient GetServiceClient(string endPointUrl, bool https)
    {
      BasicHttpBinding binding = new BasicHttpBinding();
      EndpointAddress endPoint = new EndpointAddress(endPointUrl);

      binding.Name = "XTMWebServiceSoapBinding";
      if (https)
      {
        binding.Security.Mode = BasicHttpSecurityMode.Transport;
        binding.MessageEncoding = WSMessageEncoding.Mtom;
      }
      binding.MaxReceivedMessageSize = 2147483647;

      return new ProjectManagerMTOMWebServiceClient(binding, endPoint);
    }
  }
}
