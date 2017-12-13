using LjungbergIt.Xtm.Connector.Helpers;
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
using Sitecore.Data.Managers;

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

          //Build the list of related content items
          TranslationItem relatedContent = new TranslationItem();
          List<TranslationItem> relatedContentItems = relatedContent.GetRelatedContentItems(contextItem);
          foreach (TranslationItem relatedItem in relatedContentItems)
          {
            ListItem listItem = GetListItem(relatedItem, 1, true, true, null, true, "");
            cblIncludeRelatedContentItems.Items.Add(listItem);
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
        //labelErrorMessage.Text = "test" + contextItem.Name;

        //Create an item list of items to translate, if sub items needs to be added there will be more than one item
        //ItemList ItemsToTranslate = new ItemList();
        List<TranslationItem> itemsToTranslate = new List<TranslationItem>();

        //Check if item has the XtmBaseTemplate. If not the item is not added for translation
        XtmBaseTemplate xtmBaseTemplate = new XtmBaseTemplate(contextItem);

        //If selected item is available for translation (have the correct base template) add it to the Item list
        if (xtmBaseTemplate.HasXtmBaseTemplate)
        {
          //Add item that needs to be translated to the item list
          TranslationItem translationItem = new TranslationItem() { sitecoreItem = contextItem };
          itemsToTranslate.Add(translationItem);
        }

        //If any items in "Include related content" have been checked, add the selected items
        foreach (ListItem listItem in cblIncludeRelatedContentItems.Items)
        {
          itemsToTranslate = GetItemsToTranslate(cblIncludeRelatedContentItems, itemsToTranslate);
        }

        //If "add subitems" have been checked, add the ones that has been selected
        if (cbAllSubItems.Checked)
        {
          itemsToTranslate = GetItemsToTranslate(cblIncludeAllSubitems, itemsToTranslate);
        }

        //If any related items to subitems have been checked, add the selected items'
        if (cblIncludeAllSubitemsRelatedItems.Items.Count > 0)
        {
          itemsToTranslate = GetItemsToTranslate(cblIncludeAllSubitemsRelatedItems, itemsToTranslate);
        }

        if (itemsToTranslate.Count == 0)
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
          string dueDate = string.Empty;
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

                dueDate = inputDueDate.Value;

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
            foreach (TranslationItem itemToTranslate in itemsToTranslate)
            {
              string result = translationQueue.AddToQueue(itemToTranslate, sourceLanguage, targetLanguagesList, xtmTemplate, addedBy, projectName, dueDate);
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

    private List<TranslationItem> GetItemsToTranslate(CheckBoxList checkBoxList, List<TranslationItem> itemsToTranslate)
    {
      CheckListHelper checkListHelper = new CheckListHelper();
      foreach (ListItem listItem in checkBoxList.Items)
      {
        if (listItem.Selected)
        {
          TranslationItem translationItem = null;
          if (listItem.Value.Contains("_"))
          {
            translationItem = checkListHelper.GetIds(listItem.Value);
          }
          else
          {
            Item itemToTranslate = masterDb.GetItem(listItem.Value);
            if (itemToTranslate != null)
            {
              translationItem = new TranslationItem() { sitecoreItem = itemToTranslate };
            }
          }
          if (translationItem != null)
          {
            if (!itemsToTranslate.Any(n => n.sitecoreItem.ID == translationItem.sitecoreItem.ID))
            {
              itemsToTranslate.Add(translationItem);
            }
          }
          else
          {
            ScLogging scLogging = new ScLogging();
            scLogging.WriteError("Error adding subitem for translation, value of checkbox is: " + listItem.Value);
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

    private ListItem GetListItem(TranslationItem translationItem, int indent, bool linkToItem, bool parentName, string className, bool isChecked, string jsFunction)
    {
      string icon = ThemeManager.GetIconImage(translationItem.sitecoreItem, 16, 16, "", "");
      string listItemName = icon + translationItem.sitecoreItem.Name;
      string listItemValue = translationItem.sitecoreItem.ID.ToString();
      if (parentName)
      {
        listItemName = listItemName + " (Relates to: " + translationItem.RelatesTo.Name + ")";
        listItemValue = listItemValue + "_" + translationItem.RelatesTo.ID;

      }
      if (linkToItem)
      {
        listItemName = listItemName + " (<a href=\"/sitecore/shell/Applications/Content Editor.aspx?fo=" + translationItem.sitecoreItem.ID.ToString() + "\" target=\"_blank\">Link to item</a>)";
      }
      ListItem listItem = new ListItem(listItemName, listItemValue, translationItem.Translatable);
      listItem.Attributes.CssStyle.Value = ("padding-left: " + indent * translationItem.SubItemLevel + "px");
      listItem.Attributes["title"] = translationItem.HelpText;
      if (className != null)
      {
        listItem.Attributes["class"] = className;
      }

      if (translationItem.Translatable)
      {
        if (isChecked)
        {
          listItem.Selected = true;
        }
        listItem.Attributes.Add("onchange", jsFunction);
      }
      return listItem;
    }

    protected void cbAllSubItems_CheckedChanged(object sender, EventArgs e)
    {
      //New code
      if (cbAllSubItems.Checked)
      {
        TranslationItem subItems = new TranslationItem();
        string itemIdString = Request.QueryString["id"];
        Item contextItem = masterDb.GetItem(itemIdString);
        List<TranslationItem> subItemsList = subItems.GetListOfSubItems(contextItem, new List<TranslationItem>(), 1);

        List<TranslationItem> allRelatedItems = new List<TranslationItem>();
        TranslationItem relatedContent = new TranslationItem();

        foreach (TranslationItem subItem in subItemsList)
        {
          ListItem listItem = GetListItem(subItem, 15, false, false, null, true, "toggleRelatedItem(this)");
          cblIncludeAllSubitems.Items.Add(listItem);

          //add related items
          List<TranslationItem> relatedContentItems = relatedContent.GetRelatedContentItems(subItem.sitecoreItem);
          foreach (TranslationItem relatedItem in relatedContentItems)
          {
            allRelatedItems.Add(relatedItem);
          }
        }

        foreach (TranslationItem relatedItem in allRelatedItems)
        {
          ListItem listItem = GetListItem(relatedItem, 1, true, true, "classsubitemrelateditem", false, "");
          cblIncludeAllSubitemsRelatedItems.Items.Add(listItem);
        }

        //NEW TREEVIEW CODE
        //TreeNode
        //NEW TREEVIEW CODE END
      }
      else
      {
        cblIncludeAllSubitems.Items.Clear();
        cblIncludeAllSubitemsRelatedItems.Items.Clear();
        //cbIncludeRelatedContentItemsFromSubItems.Checked = false;
      }
      //Old code

      //if (cbAllSubItems.Checked)
      //{
      //  TranslationItem subItems = new TranslationItem();
      //  string itemIdString = Request.QueryString["id"];
      //  Item contextItem = masterDb.GetItem(itemIdString);
      //  List<TranslationItem> subItemsList = subItems.GetListOfSubItems(contextItem, new List<TranslationItem>(), 1);

      //  foreach (TranslationItem subItem in subItemsList)
      //  {
      //    ListItem listItem = GetListItem(subItem, 15, false, false);
      //    cblIncludeAllSubitems.Items.Add(listItem);
      //  }
      //}
      //else
      //{
      //  cblIncludeAllSubitems.Items.Clear();
      //  cblIncludeAllSubitemsRelatedItems.Items.Clear();
      //  cbIncludeRelatedContentItemsFromSubItems.Checked = false;
      //}

    }

    //protected void cbIncludeRelatedContentItemsFromSubItems_CheckedChanged(object sender, EventArgs e)
    //{
    //  if (cbIncludeRelatedContentItemsFromSubItems.Checked)
    //  {
    //    List<TranslationItem> allRelatedItems = new List<TranslationItem>();
    //    foreach (ListItem listItem in cblIncludeAllSubitems.Items)
    //    {
    //      if (listItem.Selected)
    //      {
    //        Item contextItem = masterDb.GetItem(listItem.Value);
    //        if (contextItem != null)
    //        {
    //          TranslationItem relatedContent = new TranslationItem();
    //          List<TranslationItem> relatedContentItems = relatedContent.GetRelatedContentItems(contextItem);
    //          foreach (TranslationItem subItem in relatedContentItems)
    //          {
    //            allRelatedItems.Add(subItem);
    //          }
    //        }
    //      }
    //    }

    //    foreach (TranslationItem subItem in allRelatedItems)
    //    {
    //      ListItem listItem = GetListItem(subItem, 1, true, true);
    //      cblIncludeAllSubitemsRelatedItems.Items.Add(listItem);
    //    }
    //  }
    //  else
    //  {
    //    cblIncludeAllSubitemsRelatedItems.Items.Clear();
    //  }
    //}
  }

}