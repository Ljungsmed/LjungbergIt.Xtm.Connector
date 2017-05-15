using LjungbergIt.Xtm.Connector.Commands;
using LjungbergIt.Xtm.Connector.Export;
using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Jobs;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;

namespace LjungbergIt.Xtm.Connector.Buttons
{
  class StartTranslation : Command
  {
    public override void Execute(CommandContext context)
    {
      string filePath = System.Web.HttpContext.Current.Server.MapPath("~\\" + ScConstants.Misc.translationFolderName + "\\" + ScConstants.Misc.filesFortranslationFolderName + "\\");
      object[] args = new object[] { filePath };

      JobOptions jobOptions = new JobOptions(
        "XTM_StartTranslateJob", 
        "XTM Job", 
        Sitecore.Context.Site.Name,
        this,  
        "StartJob",
        args
        );

      JobManager.Start(jobOptions);

      Sitecore.Context.ClientPage.ClientResponse.Alert("Projects are being created in XTM, you can close this window and carry on ;)");

      //List<ReturnMessage> info = convert.Transform();
      //StringBuilder response = new StringBuilder();
      //if (info.Count > 0)
      //{
      //  foreach (ReturnMessage message in info)
      //  {
      //    response.Append(message.Message);
      //    response.Append("<br />");
      //  }
      //}  
      //else
      //{
      //  response.Append("No return messages TODO FIX");
      //}
      //Sitecore.Context.ClientPage.ClientResponse.Alert(response.ToString());
    }
    private void StartJob(string filePath)
    {
      ExportInfo exportInfo = new ExportInfo();
      
      List<UpdateItem> updateProps = new List<UpdateItem>();
      DateTime startTime = DateTime.Now;
      updateProps.Add(new UpdateItem { FieldIdOrName = exportInfo.StatusField, FieldValue = "Currently running" });
      updateProps.Add(new UpdateItem { FieldIdOrName = exportInfo.StartField, FieldValue = startTime.ToString("yyyyMMddThhmmss") });
      updateProps.Add(new UpdateItem { FieldIdOrName = exportInfo.ErrorsField, FieldValue = string.Empty });

      UpdateItem updateItem = new UpdateItem();
      updateItem.Update(exportInfo.ExportInfoItem, updateProps);
      updateProps.Clear();

      ConvertToXml convert = new ConvertToXml();
      List<ReturnMessage> returnMessages = convert.Transform(filePath);

      DateTime endTime = DateTime.Now;
      TimeSpan totalTime = endTime.Subtract(startTime);
      updateProps.Add(new UpdateItem { FieldIdOrName = exportInfo.StatusField, FieldValue = "Finnished" });
      updateProps.Add(new UpdateItem { FieldIdOrName = exportInfo.EndField, FieldValue = endTime.ToString("yyyyMMddThhmmss") });
      updateProps.Add(new UpdateItem { FieldIdOrName = exportInfo.TotalTime, FieldValue = totalTime.Seconds.ToString() + " Seconds"});
      //updateProps.Add(new UpdateItem { FieldIdOrName = exportInfo.InitiatedBy, FieldValue = totalTime.Seconds.ToString() });

      updateItem.Update(exportInfo.ExportInfoItem, updateProps);
      
      //long totalTime = startTime.Ticks - endTime.Ticks;
    }
  }
}
