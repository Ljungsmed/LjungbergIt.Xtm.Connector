using LjungbergIt.Xtm.Connector.AddForTranslation;
using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace LjungbergIt.Xtm.Connector.Export
{
    public class ConvertToXml
    {
        Database masterDb = ScConstants.SitecoreDatabases.MasterDb;

        public string Transform()
        {
            string returnString = "All translation jobs send for translation to XTM";

            Item queuedItemsFolder = masterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueFolder);
            StartTranslation startTranslation = new StartTranslation();

            if (queuedItemsFolder.Children.Count != 0)
            {
                Item archive = null;
                using (new SecurityDisabler())
                {

                    Item archiveFolder = masterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueArchiveFolder);
                    string itemName = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                    Item newArchive = archiveFolder.Add(itemName, ScConstants.SitecoreTemplates.TranslationQueueLanguageFolderTemplate);
                    archive = newArchive;
                    //translationFolderItem.MoveTo(archive);
                }

                foreach (Item translationFolderItem in queuedItemsFolder.GetChildren())
                {
                    string fileName = translationFolderItem.Name;

                    TranslationProperties translationProperties = new TranslationProperties();
                    translationProperties.SourceLanguage = translationFolderItem[ScConstants.SitecoreFieldIds.QueuFolderSourceLanguage];
                    translationProperties.TargetLanguage = translationFolderItem[ScConstants.SitecoreFieldIds.QueuFolderTranslateTo];
                    translationProperties.XtmTemplate = translationFolderItem[ScConstants.SitecoreFieldIds.QueuFolderXtmTemplate];
                    translationProperties.ItemId = translationFolderItem.ID.ToString();

                    ItemList queuedItemList = buildQueuedItemList(translationFolderItem, translationProperties.SourceLanguage);

                    if (queuedItemList.Count != 0)
                    {
                        string filePath = "~\\" + ScConstants.Misc.translationFolderName + "\\" + ScConstants.Misc.filesFortranslationFolderName + "\\" + fileName + ".xml";
                        string fullFilePath = System.Web.HttpContext.Current.Server.MapPath(filePath);

                        using (FileStream fs = new FileStream(fullFilePath, FileMode.Create))
                        {
                            XmlWriterSettings settings = new XmlWriterSettings();
                            settings.Indent = true;

                            XmlWriter xw = XmlWriter.Create(fs, settings);
                            UpdateItem updateItem = new UpdateItem();
                            List<UpdateItem> updateList = new List<UpdateItem>();
                            updateList.Add(new UpdateItem() { FieldIdOrName = ScConstants.SitecoreFieldIds.XtmBaseTemplateInTranslation, FieldValue = "1" });
                            //xw.WriteStartDocument();
                            xw.WriteStartElement(ScConstants.XmlNodes.XmlRoot);
                            foreach (Item queuedItem in queuedItemList)
                            {
                                Item itemInTranslation = masterDb.GetItem(queuedItem[ScConstants.SitecoreFieldNames.TranslationQueueItem_ItemId]);
                                AddElement(xw, queuedItem);
                                updateItem.Update(masterDb.GetItem(itemInTranslation.ID).ID.ToString(), updateList);
                            }
                            //xw.WriteEndDocument();
                            xw.WriteEndElement();
                            xw.Flush();
                            xw.Close();
                        }

                        //TODO get something back about success
                        //When translation file in XML have been created, a project on XTM gets created with the XML file as the translation file
                        startTranslation.SendFilesToXtm(fullFilePath, fileName, translationProperties);

                        using (new SecurityDisabler())
                        {
                            translationFolderItem.MoveTo(archive);
                        }
                    }
                }
            }
            else
            {
                returnString = "There are no translation jobs waiting";
            }

            return returnString;                       
        }

        private ItemList buildQueuedItemList(Item langFolderItem, string sourceLanguage)
        {
            ItemList queuedItemList = new ItemList();
            Language sitecoreSourceLanguage = Language.Parse(sourceLanguage);
            foreach (Item queuedItem in langFolderItem.GetChildren())
            {
                Item correctLanguageItem = masterDb.GetItem(queuedItem.ID, sitecoreSourceLanguage);
                queuedItemList.Add(correctLanguageItem);
                //ArchiveItem(langFolderItem);
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
            Language lang = Language.Parse(ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_MasterLanguage]);
            //Sitecore.Data.Version version = Sitecore.Data.Version.Parse(ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_Version]);
            Sitecore.Data.ID ItemId = new Sitecore.Data.ID(ItemToAdd[ScConstants.SitecoreFieldNames.TranslationQueueItem_ItemId]);

            Item ItemToTranslate = ScConstants.SitecoreDatabases.MasterDb.GetItem(ItemId, lang); 

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
