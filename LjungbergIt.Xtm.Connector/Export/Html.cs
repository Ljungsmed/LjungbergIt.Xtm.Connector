using LjungbergIt.Xtm.Connector.Helpers;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
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
      ReturnMessage returMessage = new ReturnMessage();
      XtmSettingsItem settings = new XtmSettingsItem();
      string HomeItemPath = settings.HomeItem.Paths.FullPath;
      StringBuilder fullUrl = new StringBuilder(settings.BaseSiteUrl);
      fullUrl.Append(translationItem.Paths.Path.Replace(HomeItemPath, ""));

      //try converting the 
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
        SetNewAttributeValue(rootNode, ".//link", "href", baseUrl);
        SetNewAttributeValue(rootNode, ".//img", "src", baseUrl);

        htmlDoc.Save(htmlFilePath);

        returMessage.Success = true;
      }
      catch (Exception e)
      {
        returMessage.Success = false;
        scLogging.WriteWarn("HTML preview file not created for Item" + translationItem.Name + ". Error: " + e.Message);
        returMessage.Message = "No preview created for " + translationItem.Name;
      }
      returMessage.Item = translationItem;
      return returMessage;
    }

    private void SetNewAttributeValue(HtmlNode rootNode, string xPath, string attributeName, string preFixValue)
    {
      HtmlNodeCollection linkNodes = rootNode.SelectNodes(xPath);
      foreach (HtmlNode node in linkNodes)
      {
        bool AttributeFound = node.GetAttributeValue(attributeName, false);
        //TODO check if attr. exists
        //TODO check if attr. already have http (full path)
        string href = node.GetAttributeValue(attributeName, "");
        node.SetAttributeValue(attributeName, preFixValue + href);
      }

      
    }
  }
}