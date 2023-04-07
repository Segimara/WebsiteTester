using WebsiteTester.Models;
using WebsiteTester.Parsers;

namespace WebsiteTester.Crawlers;

public class PageCrawler
{
    private readonly WebsiteParser _websiteParser;

    public PageCrawler(WebsiteParser websiteParser)
    {
        _websiteParser = websiteParser;
    }

    public IEnumerable<WebLink> Crawl(string url)
    {
        var startUrls = _websiteParser.Parse(url);

        var visitedUrls = GetNestedUrls(startUrls);

        return visitedUrls
            .Select(u => new WebLink
            {
                Url = u,
                IsInWebsite = true,
                IsInSitemap = false
            });
    }

    private IEnumerable<string> GetNestedUrls(IEnumerable<string> urls)
    {
        var visitedUrls = new HashSet<string>();
        var urlsToVisit = new Queue<string>(urls);

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

        return visitedUrls;
    }
}