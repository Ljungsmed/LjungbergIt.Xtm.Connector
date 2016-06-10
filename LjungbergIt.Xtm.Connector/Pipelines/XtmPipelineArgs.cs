
namespace LjungbergIt.Xtm.Connector.Pipelines
{
    class XtmPipelineArgs : Sitecore.Web.UI.Sheer.ClientPipelineArgs
    {
        public string ItemId { get; set; }
        public string ItemVersion { get; set; }
        public string ItemLanguage { get; set; }
        public bool XtmBaseTemplate { get; set; }
    }
}
