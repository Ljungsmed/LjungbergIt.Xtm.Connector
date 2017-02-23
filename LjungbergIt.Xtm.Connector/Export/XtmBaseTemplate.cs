using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;


namespace LjungbergIt.Xtm.Connector.Export
{
  public class XtmBaseTemplate
  {
    public bool Translated { get; set; }

    TemplateID XtmBaseTemplateId = new TemplateID(new ID("{62378E9D-DEA1-4DED-8FFE-A944002ABC87}"));
    string TemplateTranslatedFieldId = "{0A60DBA6-38D2-4EAE-8A26-85A3D85BC0FF}";

    //TODO create constructor overload without item argument
    public XtmBaseTemplate(Item item)
    {
      if (item != null)
      {
        if (CheckForBaseTemplate(item) == true)
        {
          CheckboxField checkboxField = item.Fields[TemplateTranslatedFieldId];
          if (checkboxField.Checked)
          {
            Translated = true;
          }
          else
          {
            Translated = false;
          }
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