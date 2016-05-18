using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Webservice;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LjungbergIt.Xtm.Connector.Import
{
    class ImportFromXml
    {
        public void CreateTranslatedContentFromProgressFolder()
        {
            Item inProgressFolder = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.TranslationInProgressFolder);
            foreach (Item progressItem in inProgressFolder.GetChildren())
            {
                long projectId;
                bool projectIdOk = long.TryParse(progressItem[ScConstants.SitecoreFieldIds.XtmProjectId], out projectId);
                if (projectIdOk)
                {
                    CreateTranslatedContent(projectId);
                }
            }
        }

        public void CreateTranslatedContent(long projectId)
        {
            XtmProject xtmProject = new XtmProject();
            bool finished = xtmProject.IsTranslationFinished(projectId);
            if (finished)
            {
                XtmHandleTranslatedContent Xtm = new XtmHandleTranslatedContent();
                List<byte[]> bytesList = Xtm.GetFileInBytes(projectId);
                if (bytesList.Count != 0)
                {
                    XmlDocument translatedXmlDoc = ConvertTranslatedBytesToXML(projectId, bytesList);
                    bool result = ReadFromXml(translatedXmlDoc);

                    if (result)
                    {
                        using (new SecurityDisabler())
                        {
                            //progressItem.Delete();
                        }
                    }
                }
            }
        }

        public XmlDocument ConvertTranslatedBytesToXML(long projectId, List<byte[]> bytesList)
        {
            XmlDocument xmlDocument = new XmlDocument();
            string fileName = "translatedContent";
            string filePath = System.Web.HttpContext.Current.Server.MapPath("~\\" + ScConstants.Misc.translationFolderName + "\\" + fileName);

            foreach (byte[] bytes in bytesList)
            {
                File.WriteAllBytes(filePath + ".zip", bytes);
                using (ZipArchive archive = ZipFile.OpenRead(filePath + ".zip"))
                {
                    IReadOnlyCollection<ZipArchiveEntry> zipEntries = archive.Entries;
                    foreach (ZipArchiveEntry entry in zipEntries)
                    {
                        entry.ExtractToFile(filePath + ".xml", true);
                        xmlDocument.Load(filePath + ".xml");                       
                    }
                }

            }
            return xmlDocument;
        }

        public bool ReadFromXml(XmlDocument xmlDocument)
        {
            try
            {
                XmlNode rootElement = xmlDocument.SelectSingleNode(ScConstants.XmlNodes.XmlRoot);
                foreach (XmlNode node in rootElement)
                {
                    if (node.Name.Equals(ScConstants.XmlNodes.XmlRootElement))
                    {
                        Item translatedItem = CreateNewTranslatedVersion(node);
                    }

                    if (node.Name.Equals(ScConstants.XmlNodes.XmlAttributeFieldName))
                    {

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                //TODO write ex to log
                return false;
            }
            
        }

        private Item CreateNewTranslatedVersion(XmlNode node)
        {
            string sitecoreItemId = node.Attributes[ScConstants.XmlNodes.XmlAttributeId].Value;
            string language = node.Attributes[ScConstants.XmlNodes.XmlAttributeLanguage].Value;
            Language SitecoreLanguage = Language.Parse(language);

            Item translationItem = ScConstants.SitecoreDatabases.MasterDb.GetItem(sitecoreItemId, SitecoreLanguage);
            Item newVersion;
            using (new SecurityDisabler())
            {
                newVersion = translationItem.Versions.AddVersion();
            }

            List<FieldObject> fieldObjects = new List<FieldObject>();

            foreach (XmlNode fieldNode in node)
            {
                FieldObject field = new FieldObject();
                field.fieldName = fieldNode.Attributes[ScConstants.XmlNodes.XmlAttributeFieldName].Value;
                field.fieldValue = fieldNode.InnerText;
                fieldObjects.Add(field);
            }

            UpdateFields(newVersion, fieldObjects);

            return (translationItem);
        }

        private void UpdateFields(Item translationItem, List<FieldObject> fieldObjects)
        {
            using (new SecurityDisabler())
            {
                translationItem.Editing.BeginEdit();
                foreach (FieldObject field in fieldObjects)
                {
                    translationItem[field.fieldName] = field.fieldValue;
                }                
                translationItem.Editing.EndEdit();
            }
        }
    }
}
