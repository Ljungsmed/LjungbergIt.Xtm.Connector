using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using System.Collections.Generic;

//TODO Move this class outside Export as it is used by the Metadata code as well
namespace LjungbergIt.Xtm.Connector.Export
{
  public class XtmBaseTemplate
  {
    public bool Translated { get; set; }
    public bool InTranslation { get; set; }
    public bool HasXtmBaseTemplate { get; set; }

    

    public struct Ids
    {
      public static readonly string TemplateId = "{62378E9D-DEA1-4DED-8FFE-A944002ABC87}";
      public static readonly string TranslatedFieldId = "{0A60DBA6-38D2-4EAE-8A26-85A3D85BC0FF}";
      public static readonly string InTranslationFieldId = "{C33EEDFF-6244-4453-8C99-F5DCC7AF3E9D}";
      public static readonly string TranslationDateFieldId = "{F6276376-28A9-4BE4-AAEA-C20DBC4A4F0C}";
      public static readonly string TranslatedFromFieldId = "{257532BF-69D3-4A18-9F3E-8443BA5A0993}";
    }   

    //TODO create constructor overload without item argument
    public XtmBaseTemplate(Item item)
    {
      if (item != null)
      {

        if (CheckForBaseTemplate(item))
        {
          HasXtmBaseTemplate = true;
          CheckboxField translatedField = item.Fields[Ids.TranslatedFieldId];
          if (translatedField.Checked)
          {
            Translated = true;
          }
          else
          {
            Translated = false;
          }

          CheckboxField inTranslationField = item.Fields[Ids.InTranslationFieldId];
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
      TemplateID XtmBaseTemplateId = new TemplateID(new ID(XtmBaseTemplate.Ids.TemplateId));
      bool hasXtmBaseTemplate = TemplateManager.GetTemplate(item).DescendsFrom(XtmBaseTemplateId); 
      return hasXtmBaseTemplate;
    }
  }
}