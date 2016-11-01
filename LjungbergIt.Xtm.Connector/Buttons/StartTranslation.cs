using LjungbergIt.Xtm.Connector.Commands;
using LjungbergIt.Xtm.Connector.Export;
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
            string info = convert.Transform();
            //SheerResponse.Alert(info, false, "what is this?");
            Sitecore.Context.ClientPage.ClientResponse.Alert(info);
        }
    }
}
