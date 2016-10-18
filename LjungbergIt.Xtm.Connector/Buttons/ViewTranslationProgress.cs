using LjungbergIt.Xtm.Connector.Pipelines;
using Sitecore.Shell.Framework.Commands;

namespace LjungbergIt.Xtm.Connector.Buttons
{
    class ViewTranslationProgress : Command
    {
        public override void Execute(CommandContext context)
        {
            //SheerResponse.ShowModalDialog("/XtmFiles/ViewTranslationProgress.aspx", "1200", "800", "testing my message", false, "600", "400");
            XtmPipelineArgs args = new XtmPipelineArgs();
            Sitecore.Context.ClientPage.Start("XtmConnectorPipelineTranslationProgress", args);
        }
    }
}
