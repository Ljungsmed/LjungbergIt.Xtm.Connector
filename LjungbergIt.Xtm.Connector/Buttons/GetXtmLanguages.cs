using LjungbergIt.Xtm.Connector.Import;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace LjungbergIt.Xtm.Connector.Buttons
{
    class GetXtmLanguages : Command
    {
        public override void Execute(CommandContext context)
        {
            XtmLanguages xtmLanguages = new XtmLanguages();

            xtmLanguages.CreateAllXtmLanguages();

            //SheerResponse.Alert(info, false);
        }
    }
}
