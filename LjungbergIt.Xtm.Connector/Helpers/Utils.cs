using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Connector.LanguageHandling
{
  class Utils
  {
    public string FormatItemName(string itemName)
    {
      itemName = itemName.Replace("{", "");
      itemName = itemName.Replace("}", "");
      itemName = itemName.Replace(",", "");
      return itemName;
    }

    public bool ValidateItemName(string itemName)
    {       
      Regex regex = new Regex(@"^[a-zA-Z0-9_\- ]*$");
      return (regex.IsMatch(itemName));
    }

  }
}
