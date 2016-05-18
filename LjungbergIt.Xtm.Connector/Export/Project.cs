using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Connector.Export
{
    public class Project
    {
        public void CreateProgressItem(string xtmProjectId)
        {
            if (!xtmProjectId.Contains("ERROR"))
            {
                Item progressFolder = ScConstants.SitecoreDatabases.MasterDb.GetItem(ScConstants.SitecoreIDs.TranslationInProgressFolder);

                using (new SecurityDisabler())
                {
                    Item progressItem = progressFolder.Add(xtmProjectId, ScConstants.SitecoreTemplates.TranslationInProgressTemplate);
                    progressItem.Editing.BeginEdit();
                    progressItem[ScConstants.SitecoreFieldIds.XtmProjectId] = xtmProjectId;
                    progressItem.Editing.EndEdit();
                }
            }
        }
    }
}
