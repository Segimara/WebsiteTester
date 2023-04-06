using HtmlAgilityPack;
using Microsoft.VisualBasic;
using System;
using System.Diagnostics.Metrics;
using System.Linq;
using WebsiteTester.Services.Parsers;

namespace WebsiteTester.Crawlers
{
    public class OnPageCrawler 
    {
        private WebsiteParser _websiteParser;

        public OnPageCrawler(WebsiteParser websiteParser)
        {
            _websiteParser = websiteParser;
        }

        public IEnumerable<string> Crawl(string _url)
        {
            var startUrls = _websiteParser.Parse(_url);

            var visitedUrls = new HashSet<string>();
            var urlsToVisit = new Queue<string>(startUrls);

            while (urlsToVisit.Count > 0)
            {
                var url = urlsToVisit.Dequeue();
                
                IEnumerable<string> linksInUrlThoseNotParsedYet = null;
                try
                {
                    linksInUrlThoseNotParsedYet = _websiteParser.Parse(url).Except(visitedUrls);
                }
                catch (Exception)
                {
                    continue;
                }

                foreach (var link in linksInUrlThoseNotParsedYet)
                {
                    urlsToVisit.Enqueue(link);
                }
            }

            return visitedUrls;
        }
        
    }
}