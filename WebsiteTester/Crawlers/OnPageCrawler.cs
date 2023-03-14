using HtmlAgilityPack;
using System;
using WebsiteTester.Interfaces;

namespace WebsiteTester.Crawlers
{
    public class OnPageCrawler : IPageCrawler
    {
        public IEnumerable<string> Crawl(string url)
        {
            var basiUrl = new Uri(url);
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var links = doc.DocumentNode.Descendants("a")
                            .Select(a => a.GetAttributeValue("href", null))
                            .Where(href => ValidateUrl(href));
            var correctUrls = links.Distinct().GetCorrectUrls(basiUrl);
            var uniqueUrls = correctUrls.Distinct();
            return uniqueUrls;   
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

    }
}