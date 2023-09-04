using WebsiteTester.Application.Features.WebsiteTester.Crawlers;
using WebsiteTester.Crawler.Parsers;
using WebsiteTester.Crawler.Utility;
using WebsiteTester.Domain.InternalModels;

namespace WebsiteTester.Crawler.Crawlers;

public class WebsiteCrawler : IWebsiteCrawler
{
    private readonly TimeMeterUtility _renderTimeMeter;
    private readonly SitemapParser _siteMapParser;
    private readonly PageCrawler _webCrawler;

    public WebsiteCrawler(SitemapParser siteMapParser, PageCrawler webCrawler,
        TimeMeterUtility renderTimeMeter)
    {
        _renderTimeMeter = renderTimeMeter;
        _webCrawler = webCrawler;
        _siteMapParser = siteMapParser;
    }

    public virtual async Task<IEnumerable<WebLink>> GetUrlsAsync(string url)
    {
        var onPageUrls = _webCrawler.Crawl(url);
        var sitemapUrls = await _siteMapParser.ParseAsync(url);

        var uniqueUrls = onPageUrls.Concat(sitemapUrls)
            .GroupBy(x => x.Url)
            .Select(g => new WebLink
            {
                Url = g.Key,
                IsInSitemap = g.Any(x => x.IsInSitemap),
                IsInWebsite = g.Any(x => x.IsInWebsite)
            });

        var testResults = await _renderTimeMeter.TestRenderTimeAsync(uniqueUrls);

        return testResults;
    }

}