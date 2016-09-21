using LjungbergIt.Xtm.Webservice.XtmServiceReference;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Text;

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

        public ProjectManagerMTOMWebServiceClient GetServiceClient(string endPointUrl)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endPoint = new EndpointAddress(endPointUrl);

            binding.Name = "XTMWebServiceSoapBinding";

            return new ProjectManagerMTOMWebServiceClient(binding, endPoint);
        }
    }
}
