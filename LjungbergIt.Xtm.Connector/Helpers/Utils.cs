using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Connector.Helpers
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
    }
}
