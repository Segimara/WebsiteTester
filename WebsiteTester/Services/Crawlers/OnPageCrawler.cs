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
                        linksInUrlThoseNotParsedYet = _websiteParser.Parse(url).Except(queue).Except(alredyParsed).Except(alredyReceived);
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
        
    }
}