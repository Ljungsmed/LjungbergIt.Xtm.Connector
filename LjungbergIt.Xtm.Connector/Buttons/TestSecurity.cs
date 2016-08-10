using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Buttons
{
    class TestSecurity : Command
    {
        public override void Execute(CommandContext context)
        {
            SheerResponse.ShowModalDialog("/TestSecurity.aspx", false);
        }
    }
}
