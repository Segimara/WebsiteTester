using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WebsiteTester.Models;
using WebsiteTester.Parsers;

namespace WebsiteTester.Crawlers
{
    public class OnPageCrawler
    {
        private readonly WebsiteParser _websiteParser;

        public OnPageCrawler(WebsiteParser websiteParser)
        {
            _websiteParser = websiteParser;
        }

        public IEnumerable<WebLinkModel> Crawl(string url)
        {
            var startUrls = _websiteParser.Parse(url);

            var visitedUrls = new HashSet<string>();
            var urlsToVisit = new Queue<string>(startUrls);

            while (urlsToVisit.Count > 0)
            {
                IEnumerable<string> linksToParse = null;

                var urlToParse = urlsToVisit.Dequeue();
                visitedUrls.Add(urlToParse);
                try
                {
                    linksToParse = _websiteParser.Parse(urlToParse)
                        .Except(visitedUrls)
                        .Except(urlsToVisit);
                }
                catch (Exception)
                {
                    continue;
                }

                foreach (var link in linksToParse)
                {
                    urlsToVisit.Enqueue(link);
                }
            }

            return visitedUrls
                .Select(u => new WebLinkModel()
                {
                    Url = u,
                    IsInWebsite = true,
                    IsInSitemap = false,
                });
        }
    }
}