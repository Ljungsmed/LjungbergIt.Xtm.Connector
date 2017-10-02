using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Collections;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjungbergIt.Xtm.Connector.Search
{
  public class ItemSearch
  {
    public ItemList FindItems(string path, string fieldName, string searchFor)
    {
      ItemList resultItems = new ItemList();

      ID pathId = new ID(path);
      
      var index = ContentSearchManager.GetIndex("sitecore_master_index");
      using (var context = index.CreateSearchContext())
      {
        var queryable = context.GetQueryable<SearchResultItem>()
           .Where(x => x.Paths.Contains(pathId))
           .Where(x => x[fieldName.ToLower()].Contains(searchFor))
           ;
        foreach (var result in queryable)
        {
          resultItems.Add(ScConstants.SitecoreDatabases.MasterDb.GetItem(result.ItemId));
        }
      }

      return resultItems;
    }
  }
}