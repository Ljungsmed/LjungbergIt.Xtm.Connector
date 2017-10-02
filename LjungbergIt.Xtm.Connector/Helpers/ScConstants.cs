using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Connector.Helpers
{
  class ScConstants
  {
    public struct SitecoreIDs
    {
      public static readonly string TranslationQueueFolder = "{2A38CC02-5466-4A5E-95AE-E16E5D06D1EC}";
      public static readonly string TranslationQueueArchiveFolder = "{0432E519-9A62-4D03-A0BB-7BFB83E43646}";
      public static readonly string TranslationInProgressFolder = "{D400D345-F302-4EAD-8B16-673094960139}";
      public static readonly string XtmSettingsItem = "{16952019-98B9-43EB-B1AB-18CD6663BE28}";
      public static readonly string XtmSettingsXtmTemplateFolder = "{7EA4644E-4AE7-4D7F-B807-5D3E51444FAD}";
      public static readonly string XtmSettingsLanguageMappingFolder = "{7D96664A-4CB2-4847-9C41-479DF9AD43F5}";
      public static readonly string XtmSettingsLanguageFolder = "{D3B57D66-1FE4-43C6-8EC5-2D93A21E0471}";
      public static readonly string XtmSettingsDocumentationFolder = "{ECC6B7F8-69C6-4CE6-857A-BABDD68D2FC0}";
      public static readonly string TranslationDataFolder = "{DF5AC8B4-9C6A-42EA-9E39-3D6C71C0C5EA}";
    }

    public struct SitecoreWorkflowIDs
    {
      public static readonly string XtmWorkflow = "{115CBA9C-D124-49D5-B9D1-314B32155AAB}";
      public static readonly string XtmWorkflowStateAwaiting = "{C8CB5D17-7F02-4A34-B2AE-36D1DB4A7089}";
    }

    public struct SitecoreXtmTemplateFieldIDs
    {
      //TODO move all these to Export/XtmBaseTemplate.cs
      public static readonly string Translated = "{0A60DBA6-38D2-4EAE-8A26-85A3D85BC0FF}";
      public static readonly string TranslatedDate = "{F6276376-28A9-4BE4-AAEA-C20DBC4A4F0C}";
      public static readonly string AddedToTranslateionBy = "{974F434B-6C07-4E62-9350-719D65BABFFA}";
      public static readonly string TranslatedFrom = "{257532BF-69D3-4A18-9F3E-8443BA5A0993}";
      public static readonly string InTranslation = "{C33EEDFF-6244-4453-8C99-F5DCC7AF3E9D}";
    }

    public struct SitecoreDatabases
    {
      public static readonly Database MasterDb = Database.GetDatabase("master");
      public static readonly Database WebDb = Database.GetDatabase("web");
      public static readonly Database ContextDb = Sitecore.Context.Database;
    }

    public struct SitecoreStandardFieldNames
    {
      public static readonly string Workflow = "__Workflow";
      public static readonly string WorkflowState = "__Workflow state";
      public static readonly string CreatedBy = "__Created by";
      public static readonly string Revision = "__Revision";

    }

    public struct SitecoreTemplates
    {
      //public static readonly TemplateID TemplateName = new TemplateID(new ID("Id of the template"));
      public static readonly TemplateID TranslationQueueItemTemplate = new TemplateID(new ID("{C4A16DCB-2659-42D6-A641-89A83F34F358}"));
      public static readonly TemplateID TranslationQueueLanguageFolderTemplate = new TemplateID(new ID("{83825651-C491-4B03-9229-27A4954B38A9}"));
      public static readonly TemplateID TranslationInProgressTemplate = new TemplateID(new ID("{2D0F6780-6E62-428D-A52B-B560BDE0C8DC}"));      
      public static readonly TemplateID XtmTemplate = new TemplateID(new ID("{417CBA9B-9A28-4BF5-A1E0-1D5C1B513DB0}"));
      public static readonly TemplateID XtmLanguage = new TemplateID(new ID("{3F8ECEFB-B8AB-424E-9FD5-9ED3EE2624B6}"));
    }

    public struct SitecoreFieldNames
    {
      //public static readonly string MainTitle = "main title";
      //public static readonly string TranslationQueueItem_ItemId = "ItemId";
      //public static readonly string TranslationQueueItem_MasterLanguage = "{0E5B813D-C8AD-4C8B-80A8-5F85E2F981CA}";
      public static readonly string TranslationQueueItem_TranslateTo = "TranslateTo";
      public static readonly string TranslationQueueItem_Version = "Version";
      public static readonly string Settings_MasterLanguage = "DefaultMasterLanguage";
      public static readonly string SitecoreLanguageItem_Iso = "iso";
      public static readonly string TranslateTo = "Translate To";
    }

    public struct SitecoreFieldIds
    {
      public static readonly string XtmProjectId = "{1EDFBAF9-9A37-4419-8C51-58882398F041}";
      public static readonly string XtmSettingsDefaultXtmTemplate = "{02EAC8A6-5B64-4F9C-84E6-6340BAB0E243}";
      public static readonly string XtmSettingsDefaultSourceLanguage = "{5FD31C49-7C69-406E-B221-12D3E2E9D74E}";
      public static readonly string XtmSettingsClient = "{786D97AB-DE6D-44F9-B9B8-626A9ADCF79C}";
      public static readonly string XtmSettingsUserId = "{4040E782-75B1-47E7-BF16-BA78792FE90F}";
      public static readonly string XtmSettingsPasword = "{C04387E1-8073-438E-875E-597810C776B3}";
      public static readonly string XtmSettingsCustomer = "{E2286B50-BC26-42F3-B17F-DB280E62E42B}";
      public static readonly string XtmSettingsEndpoint = "{4F43B55C-813D-4FCB-AEFE-8A45A441EEDD}";
      //public static readonly string QueuFolderSourceLanguage = "{49BB7FBD-0C7E-41BA-8E0B-0FCA7F8BB538}";
      public static readonly string QueuFolderTranslateTo = "{D61CDBB9-1056-440B-B02B-38AEAB882AB0}";
      //public static readonly string QueuFolderXtmTemplate = "{00C42DB8-293A-41A2-9ED9-12244523EA9C}";
      //public static readonly string XtmBaseTemplateTranslated = "{0A60DBA6-38D2-4EAE-8A26-85A3D85BC0FF}";
      //public static readonly string XtmBaseTemplateInTranslation = "{C33EEDFF-6244-4453-8C99-F5DCC7AF3E9D}";
      //public static readonly string XtmBaseTemplateTranslatedDate = "{F6276376-28A9-4BE4-AAEA-C20DBC4A4F0C}";
      //public static readonly string XtmBaseTemplateAddedToTranslationBy = "{974F434B-6C07-4E62-9350-719D65BABFFA}";
      public static readonly string XtmTemplateName = "{6B857053-5045-4C75-BFE2-E2D0FDD88B7E}";
      public static readonly string XtmTemplateId = "{D2B211E6-414F-48FB-836E-67A8A97B5B3C}";
      public static readonly string SitecoreLanguageName = "{471FDD58-5685-4BD4-934C-D82D6D37FDF8}";
      public static readonly string XtmLanguageName = "{C300CF84-9D5F-4E5C-9EFD-82812B15713F}";
      //public static readonly string TranslationQueueItem_AddedBy = "{5676881D-A27B-4536-8C53-A7C90E76AC26}";
    }

    //TODO move all XtmSettings from above
    public struct XtmSettingsTemplate
    {
      public static readonly string https = "{D2079525-D82D-4C33-B2B3-B5FA1FACD890}";
      public static readonly string Endpoint = "{4F43B55C-813D-4FCB-AEFE-8A45A441EEDD}";
      public static readonly string CallbackUrl = "{27B0373F-6B96-496C-820E-FF3447E67A16}";
      public static readonly string ProjectNamePrefix = "{230BD66E-EB4A-471A-BC7A-936354E18C11}";
      //public static readonly string IntegrationKey = "{7D3AC5A3-7AE0-41C5-A046-F95282DFF883}";
      public static readonly string BaseSiteUrl = "{9DA93B5C-D933-40B6-A357-E41C6FC336FD}";
    }

    public struct XtmLanguageTemplate
    {
      public static readonly string LanguageName = "{E8A5EADE-877A-4F22-A92C-1BDE0E52C234}";
    }
    public struct XtmQueueProjectTemplateFolder
    {
      public static readonly string TargetLanguages = "{868DD612-F45C-4B70-9C13-3F29B2C8CF2B}";
      public static readonly string XTMProjectName = "{F0BB1E31-D56D-40A2-9008-862217657776}";
      public static readonly string SourceLanguage = "{49BB7FBD-0C7E-41BA-8E0B-0FCA7F8BB538}";
      public static readonly string XtmTemplate = "{00C42DB8-293A-41A2-9ED9-12244523EA9C}";
    }

    public struct XtmConnectorBaseTemplate
    {
      //public static readonly string TranslatedRevision = "{3D08F793-798D-4097-90BE-1311400A43E9}";
    }

    public struct XtmDocumentationSectionTemplate
    {
      public static readonly string SectionHeading = "{6C9BB079-D699-4E25-A521-C2A9FB056C58}";
      public static readonly string SectionText = "{597C4989-17EF-4C52-BD1A-A1796A79BB2B}";
    }

    //public struct XtmTranslationQueueItemTemplate
    //{
    //  public static readonly string ItemId = "{F226C914-A62E-4B58-A2FC-B59CBD4C00F8}";
    //  public static readonly string AddedBy = "{5676881D-A27B-4536-8C53-A7C90E76AC26}";
    //}

    public struct Misc
    {
      //public static readonly string EmailVaidationString = "\\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,}\\b";
      public static readonly string translationFolderName = "XTMData";
      public static readonly string filesFortranslationFolderName = "TranslationFiles";
      public static readonly string filesForImportFolderName = "TranslatedFiles";
      public static readonly string CallbackParameter = "xtmProjectId";
      public static readonly string CallbackUrl = "/xtmfiles/xtmcallback.aspx";
    }

    public struct XmlNodes
    {
      //public static readonly TemplateID TemplateName = new TemplateID(new ID("Id of the template"));
      public static readonly string XmlRoot = "XtmSitecoreTranslation";
      public static readonly string XmlRootElement = "SitecoreItem";
      public static readonly string XmlElementField = "field";
      public static readonly string XmlAttributeId = "id";
      public static readonly string XmlAttributeLanguage = "language";
      public static readonly string XmlAttributeVersion = "version";
      public static readonly string XmlAttributeSourceLanguage = "sourcelangauge";
      public static readonly string XmlAttributeFieldName = "fieldName";
      public static readonly string XmlAttributeFieldType = "fieldType";
      public static readonly string XmlAttributeAddedBy = "addedBy";

    }
  }
}
