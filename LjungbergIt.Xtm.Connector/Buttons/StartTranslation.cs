using Sitecore.Shell.Framework.Commands;
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
            Sitecore.Context.ClientPage.ClientResponse.Alert("Testing");
        }
    }
}
