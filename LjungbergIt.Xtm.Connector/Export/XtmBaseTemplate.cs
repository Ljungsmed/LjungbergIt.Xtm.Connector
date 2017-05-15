using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

//TODO Move this class outside Export as it is used by the Metadata code as well
namespace LjungbergIt.Xtm.Connector.Export
{
  public class XtmBaseTemplate
  {
    public bool Translated { get; set; }
    public bool InTranslation { get; set; }
    public bool HasXtmBaseTemplate { get; set; }

    TemplateID XtmBaseTemplateId = new TemplateID(new ID("{62378E9D-DEA1-4DED-8FFE-A944002ABC87}"));
    string TemplateTranslatedFieldId = "{0A60DBA6-38D2-4EAE-8A26-85A3D85BC0FF}";
    string TemplateInTranslationFieldId = "{C33EEDFF-6244-4453-8C99-F5DCC7AF3E9D}";

    //TODO create constructor overload without item argument
    public XtmBaseTemplate(Item item)
    {
      if (item != null)
      {
        if (CheckForBaseTemplate(item) == true)
        {
          HasXtmBaseTemplate = true;
          CheckboxField checkboxField = item.Fields[TemplateTranslatedFieldId];
          if (checkboxField.Checked)
          {
            Translated = true;
          }
          else
          {
            Translated = false;
          }

          CheckboxField inTranslationField = item.Fields[TemplateInTranslationFieldId];
          if (inTranslationField.Checked)
          {
            InTranslation = true;
          }
          else
          {
            InTranslation = false;
          }
        }
        else
        {
          HasXtmBaseTemplate = false;
        }
      }      
    }

    public bool CheckForBaseTemplate(Item item)
    {
      bool hasXtmBaseTemplate = false;
      foreach (TemplateItem baseTemplate in item.Template.BaseTemplates)
      {
        if (baseTemplate.ID == XtmBaseTemplateId)
        {
          hasXtmBaseTemplate = true;
        }
      }
      return hasXtmBaseTemplate;
    }
  }
}