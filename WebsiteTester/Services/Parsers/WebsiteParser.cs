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

            if (!doc.DocumentNode.Descendants().Any())
            {
                throw new Exception("it is not a html document");
            }

            return doc.DocumentNode.Descendants("a")
                .Select(a => a.GetAttributeValue("href", null))
                .Where(href => ValidateOnlyUrls(href))
                .GetCorrectUrls(url)
                .Distinct()
                .ToList();
        }

        private bool ValidateOnlyUrls(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            bool isRelativeLink = url.StartsWith("/") || url.StartsWith(".");
            bool isAbsoluteLink = url.StartsWith("http") || url.StartsWith("https");
            bool isLocalLink = url.StartsWith("#");

            return isRelativeLink || isAbsoluteLink || isLocalLink;
        }
    }
}
