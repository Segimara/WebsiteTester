using WebsiteTester.Common.Interfaces;
using WebsiteTester.Domain;
using WebsiteTester.Models;
using WebsiteTester.Parsers;
using WebsiteTester.Services;

namespace WebsiteTester.Crawlers;

public class WebsiteCrawler
{
    private readonly IWebsiteTesterDbContext _dbContext;
    private readonly TimeMeterService _renderTimeMeter;
    private readonly SitemapParser _siteMapParser;
    private readonly PageCrawler _webCrawler;

    public WebsiteCrawler(IWebsiteTesterDbContext dbContext, SitemapParser siteMapParser, PageCrawler webCrawler,
        TimeMeterService renderTimeMeter)
    {
        _dbContext = dbContext;
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

        await SaveResultsAsync(url, testResults);

        return testResults;
    }
    private async Task SaveResultsAsync(string testedUrl, IEnumerable<WebLink> testResults)
    {
        var testedLink = _dbContext.TestedLink.FirstOrDefault(u => u.Url == testedUrl);
        if (testedLink == null)
        {
            testedLink = new TestedLink
            {
                Url = testedUrl
            };
        }

        var webLinks = testResults.Select(r => new LinkTestResult
        {
            TestedLink = testedLink,
            Id = Guid.NewGuid(),
            Url = r.Url,
            IsInSitemap = r.IsInSitemap,
            IsInWebsite = r.IsInWebsite,
            RenderTimeMilliseconds = r.RenderTimeMilliseconds,
            CreatedOn = DateTimeOffset.Now,
        });

        await _dbContext.LinkTestResult.AddRangeAsync(webLinks);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }
}