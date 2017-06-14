using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Webservice
{
  public class XtmJob
  {
    public byte[] TranslationFileInBytes { get; set; }
    public string TargetLanguage { get; set; }
  }
}
