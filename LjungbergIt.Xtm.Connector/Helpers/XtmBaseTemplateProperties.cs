using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO Delete! not being used. Use Export/XtmBaseTemplate.cs instead 
namespace LjungbergIt.Xtm.Connector.Helpers
{
  public class XtmBaseTemplateProperties
  {
    public bool IsBaseTemplate { get; set; }
    public bool Translated { get; set; }
    public DateTime TranslatedDate { get; set; }
    public string AddedToTranslationBy { get; set; }
    public bool InTranslation { get; set; }

    private string InTranslationFieldId = "{C33EEDFF-6244-4453-8C99-F5DCC7AF3E9D}";
    public XtmBaseTemplateProperties GetXtmBaseTemplateProperties(Item currentItem)
    {
      XtmBaseTemplateProperties properties = new XtmBaseTemplateProperties();

      

      return properties;
    }
  }
}
