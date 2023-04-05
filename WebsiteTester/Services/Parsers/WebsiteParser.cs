using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebsiteTester.Services.Parsers
{
    public class WebsiteParser
    {
        public List<string> Parse(string url)
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
                .GetCorrectUrls(url)
                .Distinct()
                .ToList();
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
