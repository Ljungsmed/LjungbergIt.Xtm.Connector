using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace LjungbergIt.Xtm.Connector.Buttons
{
    class GetXtmTemplates : Command
    {
        public override void Execute(CommandContext context)
        {
            Import.GetXtmTemplates getTemplates = new Import.GetXtmTemplates();
            string info = getTemplates.TemplatesFromXtm();
            SheerResponse.Alert(info, false);
        }
    }
}
