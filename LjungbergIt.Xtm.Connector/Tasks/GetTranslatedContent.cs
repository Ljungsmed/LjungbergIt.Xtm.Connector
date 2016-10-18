using LjungbergIt.Xtm.Connector.Import;
using Sitecore.Data.Items;

namespace LjungbergIt.Xtm.Connector.Tasks
{
    class GetTranslatedContent
    {
        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            ImportFromXml importFromXml = new ImportFromXml();
            importFromXml.CreateTranslatedContentFromProgressFolder();

            Sitecore.Diagnostics.Log.Info("XTM: Getting translated content finished", this);
        }
    }
}
