using LjungbergIt.Xtm.Webservice.XtmServiceReference;
using System;
using System.Collections.Generic;

using System.ServiceModel;

namespace LjungbergIt.Xtm.Webservice
{
  public class XtmTemplate
  {
    public bool XtmTemplateFound { get; set; }
    public string XtmTemplateName { get; set; }
    public long XtmTemplateId { get; set; }
    public string XtmTemplateCustomerName { get; set; }
    public string XtmTemplateMessage { get; set; }

    public List<XtmTemplate> GetTemplates(string xtmClient, long userId, string password, long CustomerId, string webServiceEndPoint, bool https)
    {
      List<XtmTemplate> templateList = new List<XtmTemplate>();
      XtmProject project = new XtmProject { Client = xtmClient, UserId = userId, Password = password };
      XtmWebserviceAccess xtmAccess = new XtmWebserviceAccess();
      loginAPI login = xtmAccess.Login(project);

      xtmCustomerDescriptorAPI[] customerDescriptorList = new xtmCustomerDescriptorAPI[] { xtmAccess.XtmCustomer(CustomerId) };
      xtmFindTemplateAPI xtmFindTemplate = new xtmFindTemplateAPI();
      xtmFindTemplate.customers = customerDescriptorList;
      xtmFindTemplate.scope = xtmTEMPLATESCOPEAPI.CUSTOMERS;

      ProjectManagerMTOMWebServiceClient client = xtmAccess.GetServiceClient(webServiceEndPoint, https);

      try
      {
        xtmTemplateDetailsResponseAPI[] templateResponses = client.findTemplate(login, xtmFindTemplate, null);

        foreach (xtmTemplateDetailsResponseAPI templateResponse in templateResponses)
        {
          XtmTemplate xtmTemplate = new XtmTemplate();
          xtmTemplate.XtmTemplateId = templateResponse.template.id;
          xtmTemplate.XtmTemplateName = templateResponse.template.name;
          xtmTemplate.XtmTemplateFound = true;

          templateList.Add(xtmTemplate);
        }
      }
      catch (Exception e)
      {
        XtmTemplate xtmTemplate = new XtmTemplate();
        xtmTemplate.XtmTemplateFound = false;
        xtmTemplate.XtmTemplateMessage = e.Message;
      }

      return templateList;
    }
  }
}
