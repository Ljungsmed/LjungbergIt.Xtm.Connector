using LjungbergIt.Xtm.Connector.Commands;
using LjungbergIt.Xtm.Connector.Export;
using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Connector.Buttons
{
  class StartTranslation : Command
  {
    public override void Execute(CommandContext context)
    {
      ConvertToXml convert = new ConvertToXml();
      List<ReturnMessage> info = convert.Transform();
      StringBuilder response = new StringBuilder();
      if (info.Count > 0)
      {
        foreach (ReturnMessage message in info)
        {
          response.Append(message.Message);
          response.Append("<br />");
        }
      }  
      else
      {
        response.Append("No return messages TODO FIX");
      }
      Sitecore.Context.ClientPage.ClientResponse.Alert(response.ToString());
    }
  }
}
