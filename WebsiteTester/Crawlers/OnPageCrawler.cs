using HtmlAgilityPack;
using Microsoft.VisualBasic;
using System;
using System.Diagnostics.Metrics;
using System.Linq;
using WebsiteTester.Interfaces;

namespace WebsiteTester.Crawlers
{
    public class OnPageCrawler : IPageCrawler
    {
        public IEnumerable<string> Crawl(string _url)
        {
            var startUrls = GetLinksFromPage(_url);
            var queue = new Queue<string>(startUrls);
            var alredyReceived = new HashSet<string>();
            var alredyParsed = new HashSet<string>();
            while (queue.Count > 0)
            {
                var url = queue.Dequeue();
                if (!alredyParsed.Contains(url))
                {
                    IEnumerable<string> linksInUrlThoseNotParsedYet = null;
                    try
                    {
                        linksInUrlThoseNotParsedYet = GetLinksFromPage(url).Except(queue).Except(alredyParsed).Except(alredyReceived);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    foreach (var link in linksInUrlThoseNotParsedYet)
                    {
                        queue.Enqueue(link);
                    }
                }
                
                alredyParsed.Add(url);
                if (!alredyReceived.Contains(url))
                {
                    alredyReceived.Add(url);
                    yield return url;
                }
            }
        }
        private bool ValidateUrl(string url)
        {
            return !String.IsNullOrEmpty(url) &&
                        (url.StartsWith("http")
                        || url.StartsWith("https")
                        || url.StartsWith("#")
                        || url.StartsWith("/")
                        || url.StartsWith("."));
        }
        
        private List<string> GetLinksFromPage(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(new Uri(url));
            if (!doc.Text.Contains("<!DOCTYPE html>"))
            {
                throw new Exception("it is not a html document");
            }
            return doc.DocumentNode.Descendants("a")
                            .Select(a => a.GetAttributeValue("href", null))
                            .Where(href => ValidateUrl(href))
                            .GetCorrectUrls(url).Distinct().ToList();
        }
    }
}