﻿using LjungbergIt.Xtm.Webservice.XtmServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Webservice
{
    public class XtmTemplate
    {
        public string XtmTemplateName { get; set; }
        public long XtmTemplateId { get; set; }

        public List<XtmTemplate> GetTemplates(string xtmClient, long userId, string password, long CustomerId)
        {
            List<XtmTemplate> templateList = new List<XtmTemplate>();
            XtmProject project = new XtmProject { Client = xtmClient, UserId = userId, Password = password };
            XtmWebserviceAccess xtmAccess = new XtmWebserviceAccess();
            loginAPI login = xtmAccess.Login(project);

            xtmCustomerDescriptorAPI customerDescriptor = new xtmCustomerDescriptorAPI();
            customerDescriptor.id = CustomerId;
            customerDescriptor.idSpecified = true;
            xtmCustomerDescriptorAPI[] customerDescriptorList = new xtmCustomerDescriptorAPI[] { customerDescriptor }; 
            xtmFindTemplateAPI xtmFindTemplate = new xtmFindTemplateAPI();
            //xtmFindTemplate.customers = customerDescriptorList;
            xtmFindTemplate.scope = xtmTEMPLATESCOPEAPI.ALL;

            ProjectManagerMTOMWebServiceClient client = new ProjectManagerMTOMWebServiceClient();
            xtmTemplateDetailsResponseAPI[] templateResponses = client.findTemplate(login, xtmFindTemplate, null);

            foreach (xtmTemplateDetailsResponseAPI templateResponse in templateResponses)
            {
                XtmTemplate xtmTemplate = new XtmTemplate();
                xtmTemplate.XtmTemplateId = templateResponse.template.id;
                xtmTemplate.XtmTemplateName = templateResponse.template.name;
                templateList.Add(xtmTemplate);
            }

            return templateList;
        }
    }
}