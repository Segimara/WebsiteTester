using WebsiteTester.Models;
using WebsiteTester.Parsers;
using WebsiteTester.Services;

namespace WebsiteTester.Crawlers;

public class WebsiteCrawler
{
    private readonly PageRenderTimeMeterService _renderTimeMeter;
    private readonly SitemapParser _siteMapParser;
    private readonly PageCrawler _webCrawler;

    public WebsiteCrawler(SitemapParser siteMapParser, PageCrawler webCrawler,
        PageRenderTimeMeterService renderTimeMeter)
    {
        _renderTimeMeter = renderTimeMeter;
        _webCrawler = webCrawler;
        _siteMapParser = siteMapParser;
    }

    public async Task<IEnumerable<WebLink>> GetUrls(string url)
    {
        var onPageUrls = _webCrawler.Crawl(url);
        var sitemapUrls = _siteMapParser.Parse(url);

        var uniqueUrls = onPageUrls.Concat(sitemapUrls)
            .GroupBy(x => x.Url)
            .Select(g => new WebLink
            {
                Url = g.Key,
                IsInSitemap = g.Any(x => x.IsInSitemap),
                IsInWebsite = g.Any(x => x.IsInWebsite)
            });

        return await _renderTimeMeter.TestRenderTime(uniqueUrls);
    }
}