using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LjungbergIt.Xtm.Webservice;
using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.Collections;

namespace LjungbergIt.Xtm.Connector.Import
{
    class GetXtmTemplates
    {
        public void TemplatesFromXtm()
        {
            XtmTemplate xtmGetTemplates = new XtmTemplate();

            LoginProperties login = new LoginProperties();
            string client = login.ScClient;
            long userId = login.ScUserId;
            string password = login.ScPassword;

            List<XtmTemplate> templateList = xtmGetTemplates.GetTemplates(login.ScClient, login.ScUserId, login.ScPassword, login.ScCustomer);
            if (templateList.Count != 0)
            {
                Database masterDb = ScConstants.SitecoreDatabases.MasterDb;
                Item xtmTemplateFolder = masterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsXtmTemplateFolder);
                List<string> existingTemplates = new List<string>();
                bool newTemplate = true;
                //bool deletedTemplate = false;

                foreach (Item template in xtmTemplateFolder.GetChildren())
                {
                    existingTemplates.Add(template.Name);
                }

                foreach (XtmTemplate xtmTemplate in templateList)
                {
                    if (existingTemplates.Count != 0)
                    {
                        if (existingTemplates.Contains(xtmTemplate.XtmTemplateName))
                        {
                            newTemplate = false;
                        }
                    }

                    if (newTemplate)
                    {
                        UpdateItem updateItem = new UpdateItem();
                        List<UpdateItem> updateList = new List<UpdateItem>();
                        updateList.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreFieldIds.XtmTemplateName, FieldValue = xtmTemplate.XtmTemplateName });
                        updateList.Add(new UpdateItem { FieldIdOrName = ScConstants.SitecoreFieldIds.XtmTemplateId, FieldValue = xtmTemplate.XtmTemplateId.ToString() });
                        updateItem.CreateItem(xtmTemplate.XtmTemplateName, xtmTemplateFolder, ScConstants.SitecoreTemplates.XtmTemplate, updateList);
                    }
                   

                }
            }
            

        }
    }
}
