using LjungbergIt.Xtm.Connector.Pipelines;
using Sitecore.Shell.Framework.Commands;

namespace LjungbergIt.Xtm.Connector.Buttons
{
    class AddItemForTranslation : Command
    {
        public override void Execute(CommandContext context)
        {
            XtmPipelineArgs args = new XtmPipelineArgs();
            args.ItemId = context.Items[0].ID.ToString();
            args.ItemLanguage = context.Items[0].Language.ToString();
            args.ItemVersion = context.Items[0].Version.ToString();
            
            Sitecore.Context.ClientPage.Start("XtmConnectorPipeline", args);
        }
    }
}
