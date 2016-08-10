using Sitecore.Diagnostics;

namespace LjungbergIt.Xtm.Connector.Helpers
{
    class ScLogging
    {
        string common = "LjungbergIt: ";
        public void WriteInfo(string textToLog)
        {
            Log.Info(common + textToLog, this);
        }

        public void WriteWarn(string textToLog)
        {
            Log.Warn(common + textToLog, this);
        }

        public void WriteError(string textToLog)
        {
            Log.Error(common + textToLog, this);
        }

    }
}
