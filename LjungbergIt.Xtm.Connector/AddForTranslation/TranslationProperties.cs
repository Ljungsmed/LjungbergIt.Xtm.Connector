using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Connector.Helpers
{
  public class TranslationProperties
  {
    //TODO use the ProjectName and ProjectDescription
    public string ProjectName { get; set; }
    public string ProjectDescription { get; set; }
    public string ItemId { get; set; }
    public string SourceLanguage { get; set; }
    public List<string> TargetLanguages { get; set; }
    public string XtmTemplate { get; set; }
    public string DueDate { get; set; }
  }
}
