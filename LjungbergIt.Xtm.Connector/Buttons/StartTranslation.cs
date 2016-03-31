using LjungbergIt.Xtm.Connector.Pipelines;
using Sitecore.Pipelines;
using Sitecore.Shell.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Connector.Buttons
{
    class StartTranslation : Command
    {
        public override void Execute(CommandContext context)
        {
            //NameValueCollection parameters = new NameValueCollection();
            XtmPipelineArgs args = new XtmPipelineArgs();
            //PipelineArgs args = new PipelineArgs();
            //Processor XtmProcessor = new Processor("XtmConnectorPipeline", "test", "")
            args.ItemId = context.Items[0].ID.ToString();
            args.ItemLanguage = context.Items[0].Language.ToString();
            args.ItemVersion = context.Items[0].Version.ToString();
            //args.CustomData.Add("ItemId", context.Items[0].ID.ToString());
            //parameters["ItemId"] = Sitecore.Context.Item.ID.ToString();
            //Sitecore.Context.ClientPage.ClientResponse.Alert(context.Items[0].Name);
            Sitecore.Context.ClientPage.Start("XtmConnectorPipeline", args);
        }
    }
}
