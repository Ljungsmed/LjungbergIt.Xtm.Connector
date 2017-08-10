using LjungbergIt.Xtm.Connector.AddForTranslation;
using LjungbergIt.Xtm.Connector.Export;
using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Linq;

namespace LjungbergIt.Xtm.Connector.XtmFiles
{
  public partial class AddItemForTranslation : System.Web.UI.Page
  {
    Database masterDb = ScConstants.SitecoreDatabases.MasterDb;

    protected void Page_Load(object sender, EventArgs e)
    {
      //TODO add security check so this file can only be called if Sitecore authenticated
      if (IsPostBack)
      {

      }
      else
      {
        string itemIdString = Request.QueryString["id"];
        Item contextItem = masterDb.GetItem(itemIdString);

        StringBuilder heading = new StringBuilder();

        if (contextItem != null)
        {
          XtmBaseTemplate xtmBaseTemplate = new XtmBaseTemplate(null);

          //Display the current item being added for translation

          if (xtmBaseTemplate.CheckForBaseTemplate(contextItem))
          {
            heading.Append("Add \"");
            heading.Append(contextItem.Name);
            heading.Append("\" for translation");
          }
          else
          {
            heading.Append(contextItem.Name);
            heading.Append(" is not allowed for translation, but subitems can still be added using the below checkbox");
          }

          litHeading.Text = heading.ToString();

          //Display all existing projects in a RadioButtonList

          StringBuilder sbExistingProjects = new StringBuilder();

          Item queueFolder = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.TranslationQueueFolder);
          if (queueFolder.Children.Count == 0)
          {
            divExistingProjects.Visible = false;
          }
          else
          {
            foreach (Item projectItem in queueFolder.GetChildren().Where(item => item[ScConstants.XtmQueueProjectTemplateFolder.XTMProjectName] != ""))
            {
              rblExistingProjects.Items.Add(new ListItem(projectItem[ScConstants.XtmQueueProjectTemplateFolder.XTMProjectName], projectItem.ID.ToString()));
            }
          }

          //Display the default langauge which is specified on the xtm settings item
          Language defaultSourceLanguage = GetDefaultSourceLanguage();
          StringBuilder sbSourceLanguage = new StringBuilder();
          sbSourceLanguage.Append("The defualt source language is: <strong>");
          sbSourceLanguage.Append(defaultSourceLanguage.CultureInfo.DisplayName);
          sbSourceLanguage.Append("</strong>. Change below if custom source langauge is required for this content");
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
        else
        {
          heading.Append("Error: Item with ID ");
          heading.Append(itemIdString);
          heading.Append(" was not found");
          litHeading.Text = heading.ToString();
        }
      }
    }

    //Action when the "Add" button has been clicked
    protected void btnAddForTranslation_Click(object sender, EventArgs e)
    {
      //If new projectname or existing project has not been choosen, show error and do nothing 
      if (rblExistingProjects.SelectedItem == null && txtProjectName.Text == string.Empty)
      {
        labelErrorMessage.Style.Add("color", "red");
        labelErrorMessage.Text = "Error: You have not chosen an existing project nor created a new";
      }
      else
      {
        //Get the item that needs to be added for translation
        Item contextItem = masterDb.GetItem(Request.QueryString["id"]);
        labelErrorMessage.Text = "test" + contextItem.Name;
        //Create an item list of items to translate, if sub items needs to be added there will be more than one item
        ItemList ItemsToTranslate = new ItemList();
        //Check if item has the XtmBaseTemplate. If not the item is not added for translation
        XtmBaseTemplate xtmBaseTemplate = new XtmBaseTemplate(null);
        //If selected item is available for translation (have the correct base template) add it to the Item list
        if (xtmBaseTemplate.CheckForBaseTemplate(contextItem))
        {
          //Add item that needs to be translated to the item list
          ItemsToTranslate.Add(contextItem);
        }
        //If "add subitems" have been checked, go through all subitems and add the ones which are available for translation
        if (cbAllSubItems.Checked)
        {
          ItemsToTranslate = GetChildrenToTranslate(ItemsToTranslate, contextItem);
        }
     
        if (ItemsToTranslate.Count == 0)
        {
          labelErrorMessage.Style.Add("color", "red");
          labelErrorMessage.Text = "No items were added. Both the chosen item and subitems were not eligable for translation";
        }

        else
        {
          StringBuilder info = new StringBuilder();
          TranslationQueue translationQueue = new TranslationQueue();
          //Set the source language, if no source language chosen in the drop down, use the default source language
          string sourceLanguage = ddSourceLanguage.SelectedValue;
          if (sourceLanguage.Equals(""))
          {
            sourceLanguage = GetDefaultSourceLanguage().CultureInfo.Name;
          }
          List<string> targetLanguagesList = new List<string>();
          string xtmTemplate = "NONE";
          string addedBy = Sitecore.Context.User.Name;
          string projectName = string.Empty;
          bool noErrors = true;
          bool validProjectName = true;

          if (txtProjectName.Text != string.Empty)
          {
            projectName = txtProjectName.Text;
            Utils utils = new Utils();
            validProjectName = utils.ValidateItemName(projectName);
          }

          //If existing project is chosen
          if (rblExistingProjects.SelectedItem != null)
          {
            labelErrorMessage.Text = string.Empty;
            projectName = rblExistingProjects.SelectedItem.Text;

            info.Append("Adding items for translation to existing project <strong>");
            info.Append(projectName);
            info.Append("</strong>");
            
          }
          //Else create new project based on the value in the project name text box
          else
          {
            //TODO check if project name from text box matches existing project
             
            //Display error message if no target language has been chosen AND no template has been choosen
            if (cbTargetLanguages.SelectedItem == null && ddXtmTemplate.SelectedValue == "")
            {
              labelErrorMessage.Style.Add("color", "red");
              labelErrorMessage.Text = "Error: When creating new project, you have to chose target language(s) or an XTM template";
              noErrors = false;
            }

            else
            {
              if (!validProjectName)
              {
                labelErrorMessage.Style.Add("color", "red");
                labelErrorMessage.Text = "Error: Project name can only contain normal charecters, numbers, hyphons and underscores";
              }
              else
              {
                labelErrorMessage.Text = string.Empty;

                //Set the xtm template, if none selected set the value to NONE
                xtmTemplate = ddXtmTemplate.SelectedValue;
                if (xtmTemplate.Equals(""))
                {
                  xtmTemplate = "NONE";
                }

                //Populate the list of target langauges
                foreach (ListItem listItem in cbTargetLanguages.Items)
                {
                  if (listItem.Selected == true)
                  {
                    //TODO move langaugeMapping to LanguageHandling 
                    //foreach (LanguageMapping langToMap in languageMappingList)
                    //{
                    //  if (langToMap.LangName.ToLower().Equals(targetLanguage.ToLower()))
                    //  {
                    //    targetLanguage = langToMap.XtmLangName;
                    //  }
                    //}

                    targetLanguagesList.Add(listItem.Value);
                  }
                }

                //Set the info string
                Mapping mapping = new Mapping();
                info.Append("New project created with name <strong>");
                info.Append(projectName);
                info.Append("</strong>");
                if (targetLanguagesList.Count != 0)
                {
                  info.Append("<br //>");
                  info.Append("target languages: ");
                  info.Append(mapping.TargetLanguagesString(targetLanguagesList));
                }
              }              
            }            
          }

          if (noErrors && validProjectName)
          {
            info.Append("<br //>");
            info.Append("<br //>");
            foreach (Item itemToTranslate in ItemsToTranslate)
            {
              string result = translationQueue.AddToQueue(itemToTranslate, sourceLanguage, targetLanguagesList, xtmTemplate, addedBy, projectName);
              info.Append(result);
              info.Append("<br //>");
            }

            //TODO Move up so also subitems gets added
            //if (cbTargetLanguages.SelectedItem == null)
            //{
            //  //string result = translationQueue.AddToQueue(contextItem, sourceLanguage, "NONE", xtmTemplate, addedBy, projectName);
            //  //info.Append(result);
            //  //info.Append(", the XTM Template defines the target language");
            //  info.Append("No target languages were chosen!");
            //  info.Append("<br //>");
            //}

            divChooseTranslationOptions.Visible = false;

            labelResult.Text = info.ToString();
          }          
        }       
      }      
    }

    private ItemList GetChildrenToTranslate(ItemList itemsToTranslate, Item parent)
    {
      if (parent.GetChildren() != null)
      {

        foreach (Item child in parent.GetChildren())
        {
          XtmBaseTemplate xtmBaseTemplate = new XtmBaseTemplate(child);
          if (xtmBaseTemplate.CheckForBaseTemplate(child))
          {
            if (!xtmBaseTemplate.Translated)
            {
              itemsToTranslate.Add(child);
            }
            if (child.GetChildren() != null)
            {
              GetChildrenToTranslate(itemsToTranslate, child);
            }
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
        return Language.Parse(defaultSourceLanguageItem.Name);
      }
    }
  }
}