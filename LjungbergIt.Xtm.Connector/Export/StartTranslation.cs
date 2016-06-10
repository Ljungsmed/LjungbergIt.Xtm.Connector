using LjungbergIt.Xtm.Connector.AddForTranslation;
using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Webservice;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Export
{
    public class StartTranslation
    {
        public string SendFilesToXtm(string filePath, string fileName, TranslationProperties translationProperties)
        {
            XtmCreateProject create = new XtmCreateProject();
            Project project = new Project();
            LoginProperties login = new LoginProperties();
            string returnResult = "";
                  
            List<string> result = create.Create(filePath, fileName, translationProperties.SourceLanguage, translationProperties.TargetLanguage, translationProperties.XtmTemplate, login.ScClient, login.ScUserId, login.ScPassword, login.ScCustomer );

            if (result[0].Equals("True"))
            {
                project.CreateProgressItem(result[1]);
                //List<UpdateItem> updateList = new List<UpdateItem>();
                //UpdateItem updateItem = new UpdateItem();
                //updateList.Add(new UpdateItem() { FieldIdOrName = ScConstants.SitecoreFieldIds.XtmBaseTemplateInTranslation, FieldValue = "1" });

                //bool updateSuccess = updateItem.Update(translationProperties.ItemId, updateList);
            }
            else
            {
                returnResult = result[1];
            }

            //TODO if result is not project id, handle it!            
            
            File.Delete(filePath);            

            return returnResult;
        }
    }
}
