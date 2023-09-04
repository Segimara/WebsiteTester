using Microsoft.Extensions.Logging;
using WebsiteTester.Crawler.Parsers;
using WebsiteTester.Domain.InternalModels;

namespace WebsiteTester.Crawler.Crawlers;

public class PageCrawler
{
    private readonly WebsiteParser _websiteParser;
    private readonly ILogger _logger;

    public PageCrawler(WebsiteParser websiteParser, ILogger<PageCrawler> logger)
    {
        _websiteParser = websiteParser;
        _logger = logger;
    }

    public virtual IEnumerable<WebLink> Crawl(string url)
    {
        IEnumerable<string> startUrls;
        try
        {
            startUrls = _websiteParser.Parse(url);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex.Message);

            return Enumerable.Empty<WebLink>();
        }

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

            try
            {
                linksToParse = _websiteParser.Parse(urlToParse)
                    .Except(visitedUrls)
                    .Except(urlsToVisit);
            }
            catch (Exception ex)
            {
                _logger.LogTrace(ex.Message);

                continue;
            }

            visitedUrls.Add(urlToParse);

            foreach (var link in linksToParse)
            {
                urlsToVisit.Enqueue(link);
            }
        }

        return visitedUrls;
    }
}