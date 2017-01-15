using LjungbergIt.Xtm.Connector.AddForTranslation;
using LjungbergIt.Xtm.Connector.Export;
using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI.WebControls;

namespace LjungbergIt.Xtm.Connector.XtmFiles
{
    public partial class AddItemForTranslation : System.Web.UI.Page
    {
        Database masterDb = ScConstants.SitecoreDatabases.MasterDb;       

        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                
            }
            else
            {
                Item contextItem = masterDb.GetItem(Request.QueryString["id"]); 

                //Display the current item being added for translation
                StringBuilder heading = new StringBuilder();
                heading.Append("Add \"");
                heading.Append(contextItem.Name);
                heading.Append("\" for translation");
                litHeading.Text = heading.ToString();

                //Display the default langauge which is specified on the xtm settings item
                Language defaultSourceLanguage = GetDefaultSourceLanguage();                
                StringBuilder sbSourceLanguage = new StringBuilder();
                sbSourceLanguage.Append("The defualt source language is: ");
                sbSourceLanguage.Append(defaultSourceLanguage.CultureInfo.DisplayName);
                sbSourceLanguage.Append(". Change below if custom source langauge is required for this content");
                litSourceLanguage.Text = sbSourceLanguage.ToString();

                //Build the drop down list with available Sitecore source languages to choose from if the default should be overwritten
                //Build the check box list with available Sitecore target languages to choose from
                ddSourceLanguage.Items.Add(new ListItem("", ""));
                LanguageCollection languages = masterDb.GetLanguages();
                foreach (Language language in languages)
                {
                    ListItem listItemSourceLanguage = new ListItem(language.CultureInfo.DisplayName, language.CultureInfo.ToString());
                    ListItem listItemTargetLanugage = new ListItem(language.CultureInfo.DisplayName, language.CultureInfo.ToString());

                    if (defaultSourceLanguage == language)
                    {
                        listItemTargetLanugage.Enabled = false;
                    }

                    ddSourceLanguage.Items.Add(listItemSourceLanguage);
                    cbTargetLanguages.Items.Add(listItemTargetLanugage);
                }

                //Build the dropdown list with available xtm templates
                
                using (new SecurityDisabler())
                {
                    Item xtmTemplatesFolder = masterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsXtmTemplateFolder);
                    Item xtmSettingsItem = masterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsItem);
                    Item defaultXtmTemplate = masterDb.GetItem(xtmSettingsItem[ScConstants.SitecoreFieldIds.XtmSettingsDefaultXtmTemplate]);

                    ddXtmTemplate.Items.Add(new ListItem("", ""));
                    foreach (Item xtmTemplate in xtmTemplatesFolder.GetChildren())
                    {
                        ListItem listItem = new ListItem(xtmTemplate.Name, xtmTemplate.ID.ToString());
                        if (defaultXtmTemplate != null)
                        {
                            if (defaultXtmTemplate.Name.Equals(xtmTemplate.Name))
                            {
                                listItem.Selected = true;
                            }
                        }
                        ddXtmTemplate.Items.Add(listItem);
                    }
                }
            }
        }

        //Action when the "Add" button has been clicked
        protected void btnAddForTranslation_Click(object sender, EventArgs e)
        {
            //Display error message if no target language has been chosen AND no template has been choosen
            if (cbTargetLanguages.SelectedItem == null && ddXtmTemplate.SelectedValue == "")
            {
                labelResult.Style.Add("color", "red");
                labelResult.Text = "No target language or template has been chosen";
            }

            else
            {
                //Get the item that needs to be added for translation
                Item contextItem = masterDb.GetItem(Request.QueryString["id"]);
                //Create an item list of items to translate, if sub items needs to be added there will be more than one item
                ItemList ItemsToTranslate = new ItemList();
                //Add items that needs to be translated to the item list
                ItemsToTranslate.Add(contextItem);
                if (cbAllSubItems.Checked)
                {
                    ItemsToTranslate = GetChildrenToTranslate(ItemsToTranslate, contextItem);
                }

                //Set the xtm template, if none selected set the value to NONE
                string xtmTemplate = ddXtmTemplate.SelectedValue;
                if (xtmTemplate.Equals(""))
                {
                    xtmTemplate = "NONE";
                }

                //Set the source language, if no source language chosen in the drop down, use the default source language
                string sourceLanguage = ddSourceLanguage.SelectedValue;
                if (sourceLanguage.Equals(""))
                {
                    sourceLanguage = GetDefaultSourceLanguage().CultureInfo.Name;
                }

                //set variables and initialize classes used in the foreach loop that generates a translation item per target languages, per item that needs to be translated
                string addedBy = Sitecore.Context.User.Name;
                StringBuilder info = new StringBuilder();
                LanguageMapping languageMapping = new LanguageMapping();
                List<LanguageMapping> languageMappingList = languageMapping.LanguageList();
                TranslationQueue translationQueue = new TranslationQueue();

                foreach (ListItem listItem in cbTargetLanguages.Items)
                {
                    if (listItem.Selected == true)
                    {
                        string targetLanguage = listItem.Value;

                        foreach (LanguageMapping langToMap in languageMappingList)
                        {
                            if (langToMap.LangName.ToLower().Equals(targetLanguage.ToLower()))
                            {
                                targetLanguage = langToMap.XtmLangName;
                            }
                        }

                        foreach (Item itemToTranslate in ItemsToTranslate)
                        {
                            string result = translationQueue.AddToQueue(itemToTranslate, sourceLanguage, targetLanguage, xtmTemplate, addedBy);
                            info.Append(result);
                            info.Append("<br //>");
                        }                        
                    }
                }

                if (cbTargetLanguages.SelectedItem == null)
                {
                    string result = translationQueue.AddToQueue(contextItem, sourceLanguage, "NONE", xtmTemplate, addedBy);
                    info.Append(result);
                    info.Append(", the XTM Template defines the target language");
                    info.Append("<br //>");
                }

                divChooseTranslationOptions.Visible = false;
                labelResult.Text = info.ToString();
            }
        }

        private ItemList GetChildrenToTranslate(ItemList itemsToTranslate, Item parent)
        {
            if (parent.GetChildren() != null)
            {
                foreach (Item child in parent.GetChildren())
                {
                    if (child.Fields[ScConstants.SitecoreXtmTemplateFieldIDs.Translated] != null)
                    {
                        itemsToTranslate.Add(child);
                    }                    
                    if (child.GetChildren() != null)
                    {
                       GetChildrenToTranslate(itemsToTranslate, child);
                    }
                }
            }
            return itemsToTranslate;
        }

        protected void ddSourceLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sourceLanguage;
            if (ddSourceLanguage.SelectedValue == "")
            {
                sourceLanguage = GetDefaultSourceLanguage().CultureInfo.Name;
            }
            else
            {
                sourceLanguage = ddSourceLanguage.SelectedValue;
            }
                    
            foreach (ListItem listItem in cbTargetLanguages.Items)
            {
                if (listItem.Value == sourceLanguage)
                {
                    listItem.Enabled = false;
                    listItem.Selected = false;
                }
                else
                {
                    listItem.Enabled = true;
                }
            }            
        }

        private Language GetDefaultSourceLanguage()
        {
            using (new SecurityDisabler())
            {
                Item xtmSettingsItem = masterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsItem);
                Item defaultSourceLanguageItem = masterDb.GetItem(xtmSettingsItem[ScConstants.SitecoreFieldIds.XtmSettingsDefaultSourceLanguage]);
                //Item defaultSourceLanguageItem = Database.GetDatabase("master").GetItem(xtmSettingsItem[ScConstants.SitecoreFieldIds.XtmSettingsDefaultSourceLanguage]);
                return Language.Parse(defaultSourceLanguageItem.Name);
            }
        }
    }
}