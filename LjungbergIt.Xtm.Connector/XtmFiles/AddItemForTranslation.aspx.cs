using LjungbergIt.Xtm.Connector.AddForTranslation;
using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using System;
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
                divChooseTranslationOptions.Visible = false;
                litResult.Text = "Added for translation";
            }
            else
            {
                Item contextItem = masterDb.GetItem(Request.QueryString["id"]);
                Item xtmSettingsItem = masterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsItem);

                StringBuilder heading = new StringBuilder();
                heading.Append("Add \"");
                heading.Append(contextItem.Name);
                heading.Append("\" for translation");
                litHeading.Text = heading.ToString();

                Item defaultSourceLanguageItem = masterDb.GetItem(xtmSettingsItem[ScConstants.SitecoreFieldIds.XtmSettingsDefaultSourceLanguage]);
                Language defaultSourceLanguage = Language.Parse(defaultSourceLanguageItem.Name);

                StringBuilder sbSourceLanguage = new StringBuilder();
                sbSourceLanguage.Append("The defualt source language is: ");
                sbSourceLanguage.Append(defaultSourceLanguage.CultureInfo.DisplayName);
                sbSourceLanguage.Append(". Change below if custom source langauge is required for this content");
                litSourceLanguage.Text = sbSourceLanguage.ToString();

                LanguageCollection languages = masterDb.GetLanguages();
                foreach (Language language in languages)
                {
                    ListItem listItemSourceLanguage = new ListItem(language.CultureInfo.DisplayName, language.CultureInfo.ToString());
                    ListItem listItemTargetLanugage = new ListItem(language.CultureInfo.DisplayName, language.CultureInfo.ToString());

                    ddSourceLanguage.Items.Add(listItemSourceLanguage);
                    cbTargetLanguages.Items.Add(listItemTargetLanugage);
                }

                //TODO make sure empty/no default template does not give null reference
                Item xtmTemplatesFolder = masterDb.GetItem(ScConstants.SitecoreIDs.XtmSettingsXtmTemplateFolder);
                string defaultXtmTemplate = masterDb.GetItem(xtmSettingsItem[ScConstants.SitecoreFieldIds.XtmSettingsDefaultXtmTemplate]).Name;
                ddXtmTemplate.Items.Add(new ListItem("", ""));
                foreach (Item xtmTemplate in xtmTemplatesFolder.GetChildren())
                {
                    ListItem listItem = new ListItem(xtmTemplate.Name, xtmTemplate.Name);
                    if (defaultXtmTemplate.Equals(xtmTemplate.Name))
                    {
                        listItem.Selected = true;
                    }
                    ddXtmTemplate.Items.Add(listItem);
                }
            }
        }

        protected void btnAddForTranslation_Click(object sender, EventArgs e)
        {
            
            Item contextItem = masterDb.GetItem(Request.QueryString["id"]);
            TranslationQueue translationQueue = new TranslationQueue();
            string xtmTemplate = ddXtmTemplate.SelectedValue;
            if (xtmTemplate.Equals(""))
            {
                xtmTemplate = "NONE";
            }
            foreach (ListItem listItem in cbTargetLanguages.Items)
            {
                if (listItem.Selected == true)
                {
                    translationQueue.AddToQueue(contextItem, ddSourceLanguage.SelectedValue, listItem.Value, xtmTemplate);
                }
            }
        }
    }
}