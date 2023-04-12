using WebsiteTester.Models;
using WebsiteTester.Parsers;
using WebsiteTester.Services;

namespace WebsiteTester.Crawlers;

public class WebsiteCrawler
{
    private readonly TimeMeterService _renderTimeMeter;
    private readonly SitemapParser _siteMapParser;
    private readonly PageCrawler _webCrawler;

    public WebsiteCrawler(SitemapParser siteMapParser, PageCrawler webCrawler,
        TimeMeterService renderTimeMeter)
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

        return await _renderTimeMeter.TestRenderTimeAsync(uniqueUrls);
    }
}