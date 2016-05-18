using LjungbergIt.Xtm.Connector.Helpers;
using LjungbergIt.Xtm.Webservice;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.SecurityModel;
using System;
using System.IO;
using System.Xml;

namespace LjungbergIt.Xtm.Connector.Export
{
    public class ConvertToXml
    {
        Database masterDb = ScConstants.SitecoreDatabases.MasterDb;

        public void Transform()
        {
            Item queuedItemsFolder = masterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueFolder);
            foreach (Item translationFolderItem in queuedItemsFolder.GetChildren())
            {
                string fileName = translationFolderItem.Name;
                ItemList queuedItemList = buildQueuedItemList(translationFolderItem);

                if (queuedItemList.Count != 0)
                {
                    string filePath = "~\\" + ScConstants.Misc.translationFolderName + "\\" + ScConstants.Misc.filesFortranslationFolderName + "\\" + fileName + ".xml";
                    using (FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath(filePath), FileMode.Create))
                    {
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;

                        XmlWriter xw = XmlWriter.Create(fs, settings);

                        //xw.WriteStartDocument();
                        xw.WriteStartElement(ScConstants.XmlNodes.XmlRoot);
                        foreach (Item queuedItem in queuedItemList)
                        {
                            AddElement(xw, queuedItem);
                        }
                        //xw.WriteEndDocument();
                        xw.WriteEndElement();
                        xw.Flush();
                        xw.Close();
                    }

                    using (new SecurityDisabler())
                    {
                        Item archiveFolder = masterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueArchiveFolder);
                        string itemName = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                        Item archive = archiveFolder.Add(itemName, ScConstants.SitecoreTemplates.TranslationQueueLanguageFolderTemplate);
                        //translationFolderItem.MoveTo(archive);
                    }
                }                
            }            
        }

        private ItemList buildQueuedItemList(Item langFolderItem)
        {
            ItemList queuedItemList = new ItemList();            

            foreach (Item queuedItem in langFolderItem.GetChildren())
            {
                queuedItemList.Add(queuedItem);
                //ArchiveItem(queuedItem);
            }

            return queuedItemList;
        }

        private void ArchiveItem(Item itemToArchive)
        {
            Item archiveFolder = masterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueArchiveFolder);
            using (new SecurityDisabler())
            {
                itemToArchive.MoveTo(archiveFolder); 
            }
        } 

        private XmlWriter AddElement(XmlWriter xw, Item ItemToAdd)
        {
            Language lang = Language.Parse(ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_TranslateTo]);
            Sitecore.Data.Version version = Sitecore.Data.Version.Parse(ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_Version]);
            Sitecore.Data.ID ItemId = new Sitecore.Data.ID(ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_ItemId]);

            Item ItemToTranslate = ScConstants.SitecoreDatabases.MasterDb.GetItem(ItemId, lang, version); 

            xw.WriteStartElement(ScConstants.XmlNodes.XmlRootElement);
            xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeId, ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_ItemId]);
            xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeLanguage, ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_TranslateTo]); //is this needed?
            xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeVersion, ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_Version]);

            //one for each field

            foreach (Field field in ItemToTranslate.Fields)
            {
                if (!field.Name.Contains("__"))
                {
                    xw.WriteStartElement(ScConstants.XmlNodes.XmlElementField);
                    xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeFieldName, field.Name);
                    xw.WriteAttributeString(ScConstants.XmlNodes.XmlAttributeFieldType, field.Type);
                    xw.WriteString(field.Value);
                    xw.WriteEndElement();
                }                
            }            

            xw.WriteEndElement();
            return xw;
        }
    }
}
