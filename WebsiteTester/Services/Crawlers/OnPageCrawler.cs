using System.Linq;
using WebsiteTester.Models;
using WebsiteTester.Services.Parsers;

namespace WebsiteTester.Services.Crawlers
{
    public class OnPageCrawler 
    {
        private WebsiteParser _websiteParser;

        public OnPageCrawler(WebsiteParser websiteParser)
        {
            _websiteParser = websiteParser;
        }

        public IEnumerable<WebLinkModel> Crawl(string _url)
        {
            var startUrls = _websiteParser.Parse(_url);

            var visitedUrls = new HashSet<string>();
            var urlsToVisit = new Queue<string>(startUrls);

            while (urlsToVisit.Count > 0)
            {
                IEnumerable<string> linksToParse = null;
                
                var url = urlsToVisit.Dequeue();
                visitedUrls.Add(url);
                try
                {
                    linksToParse = _websiteParser.Parse(url).Except(visitedUrls).Except(urlsToVisit);
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
                .Select(u => new WebLinkModel(u, false, true));
        }
        
    }
}