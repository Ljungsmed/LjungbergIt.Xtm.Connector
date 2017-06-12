using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data.Items;
using System;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace LjungbergIt.Xtm.Connector.Export
{
  public class Html
  {
    public ReturnMessage GenerateHtml(Item translationItem, string htmlFilePath)
    {
      ScLogging scLogging = new ScLogging();
      ReturnMessage returnMessage = new ReturnMessage();
      XtmSettingsItem settings = new XtmSettingsItem();
      string HomeItemPath = settings.HomeItem.Paths.FullPath;
      StringBuilder fullUrl = new StringBuilder(settings.BaseSiteUrl);
      //fullUrl.Append(translationItem.Paths.Path.Replace(HomeItemPath, ""));

      fullUrl.Append("/?sc_itemid=");
      fullUrl.Append(translationItem.ID.ToString());

      try
      {
        WebRequest req = WebRequest.Create(fullUrl.ToString());
        WebResponse response = req.GetResponse();
        StreamReader sr = new StreamReader(response.GetResponseStream());
        string htmlString = sr.ReadToEnd();

        string baseUrl = settings.BaseSiteUrl;
        
        //Using HtmlAgilityPack to parse the string of HTML to an HTML document where xPath can be used to find elements, attributes, etc.
        HtmlDocument htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlString);
        HtmlNode rootNode = htmlDoc.DocumentNode;

        //change all elements that references a static source to have the base url as prefix
        if (!baseUrl.Contains("https"))
        {
          baseUrl = baseUrl.Replace("http", "https");
        }
        //string baseUrlHttps = baseUrl.Replace("http", "https");
        
        SetNewAttributeValue(rootNode, ".//link", "href", baseUrl); //finds all <link href="css styles" /> and sets the full url
        SetNewAttributeValue(rootNode, ".//img", "src", baseUrl); //finds all <img> and sets the full url
        SetNewAttributeValue(rootNode, ".//script", "src", baseUrl); //finds all <scripts> and sets the full url

        htmlDoc.Save(htmlFilePath);

        returnMessage.Success = true;
      }
      catch (Exception e)
      {
        returnMessage.Success = false;
        scLogging.WriteWarn("HTML preview file not created for Item" + translationItem.Name + ". Error: " + e.Message);
        returnMessage.Message = "No preview created for " + translationItem.Name + ". Item is not published or does not have presentation details";
      }
      returnMessage.Item = translationItem;
      return returnMessage;
    }

    private void SetNewAttributeValue(HtmlNode rootNode, string xPath, string attributeName, string preFixValue)
    {
      HtmlNodeCollection linkNodes = rootNode.SelectNodes(xPath);
      foreach (HtmlNode node in linkNodes)
      {
        bool AttributeFound = node.GetAttributeValue(attributeName, false);
        //TODO check if attr. exists
        string href = node.GetAttributeValue(attributeName, "");
        if (!href.Contains("http"))
        {
          node.SetAttributeValue(attributeName, preFixValue + href);
        }        
      }      
    }
  }
}