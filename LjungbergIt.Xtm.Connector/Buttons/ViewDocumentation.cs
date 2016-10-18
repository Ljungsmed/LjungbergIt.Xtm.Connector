using Sitecore.Shell.Framework.Commands;
using LjungbergIt.Xtm.Connector.Pipelines;

namespace LjungbergIt.Xtm.Connector.Buttons
{
    class ViewDocumentation : Command
    {
        public override void Execute(CommandContext context)
        {
            XtmPipelineArgs args = new XtmPipelineArgs();
            Sitecore.Context.ClientPage.Start("XtmConnectorPipelineViewDocumentation", args);
        }
    }
}
