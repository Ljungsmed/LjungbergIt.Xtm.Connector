using LjungbergIt.Xtm.Connector.Pipelines;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Buttons
{
  public class ExcludedFields : Command
  {
    public override void Execute(CommandContext context)
    {
      //SheerResponse.Alert("info", false);
      XtmPipelineArgs args = new XtmPipelineArgs();
      Sitecore.Context.ClientPage.Start("XtmConnectorPipelineExcludedFields", args);
    }
  }
}