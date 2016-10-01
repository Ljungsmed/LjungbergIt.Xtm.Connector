using LjungbergIt.Xtm.Connector.Pipelines;
using Sitecore.Shell.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Connector.Buttons
{
    class TranslationDetailedView : Command
    {
        public override void Execute(CommandContext context)
        {
            //SheerResponse.ShowModalDialog("/XtmFiles/ViewTranslationProgress.aspx", "1200", "800", "testing my message", false, "600", "400");
            XtmPipelineArgs args = new XtmPipelineArgs();
            Sitecore.Context.ClientPage.Start("XtmConnectorPipelineTranslationDetailedView", args);
        }
    }
}
