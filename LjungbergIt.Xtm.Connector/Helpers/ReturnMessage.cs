using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Helpers
{
  public class ReturnMessage
  {
    public bool Success { get; set; }
    public string Message { get; set; }
    public Item Item { get; set; }
  }
}