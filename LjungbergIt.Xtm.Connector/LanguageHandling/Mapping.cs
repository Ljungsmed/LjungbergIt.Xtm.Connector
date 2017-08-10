using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Collections;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Helpers
{
  public class Mapping
  {
    public ItemList LanguageNamesToItems(List<string> languageNames)
    {
      ItemList languageItems = new ItemList();
      LanguageProps languageProps = new LanguageProps();

      foreach (string languageName in languageNames)
      {
        foreach (Item languageItem in languageProps.LanguageFolderItem.GetChildren())
        {
          if (languageItem.Name.Equals(languageName))
          {
            languageItems.Add(languageItem);
          }
        }
      }
      return languageItems;
    }

    public string TargetLanguagesString(List<string> targetLanguages)
    {
      StringBuilder targetLanguagesString = new StringBuilder();
      for (int i = 0; i < targetLanguages.Count; i++)
      {
        targetLanguagesString.Append(targetLanguages[i]);
        if (i == targetLanguages.Count)
        {
          targetLanguagesString.Append(" and ");
        }
        else
        {
          if (i + 1 != targetLanguages.Count)
          {
            targetLanguagesString.Append(", ");
          }
        }
      }
      return targetLanguagesString.ToString();
    }

    public List<string> SitecoreLanguageToXtmLanguage(Item[] SitecoreLanguageItems)
    {
      List<string> xtmLanguageList = new List<string>();

      LanguageMapping languageMapping = new LanguageMapping();
      List<LanguageMapping> languageMappingList = languageMapping.LanguageMappingList();

      foreach (Item languageItem in SitecoreLanguageItems)
      {
        bool languageConverted = false;
        foreach (LanguageMapping langmap in languageMappingList)
        {
          if (langmap.LangName.Equals(languageItem.Name))
          {
            xtmLanguageList.Add(langmap.XtmLangName);
            languageConverted = true;
          }
        }
        if (!languageConverted)
        {
          xtmLanguageList.Add(languageItem.Name.Replace("-", "_"));
        }
      }
      //TODO use Linq to find mapping items

      //foreach (LanguageMapping langToMap in languageMappingList)
      //{
      //  if (langToMap.LangName.ToLower().Equals(targetLanguage.ToLower()))
      //  {
      //    targetLanguage = langToMap.XtmLangName;
      //  }
      //}

      return xtmLanguageList;
    }

    public string XtmLanguageToSitecoreLanguage(string xtmLanguage)
    {
      LanguageMapping languageMapping = new LanguageMapping();
      List<LanguageMapping> languageMappingList = languageMapping.LanguageMappingList();
      string returnLanguage = xtmLanguage.Replace("_", "-");
      foreach (LanguageMapping mapping in languageMappingList)
      {
        if (mapping.XtmLangName.Equals(xtmLanguage))
        {
          returnLanguage = mapping.LangName;
        }
      }
      return returnLanguage;
    }

  }
}