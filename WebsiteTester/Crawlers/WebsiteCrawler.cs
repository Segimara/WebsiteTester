using WebsiteTester.Models;
using WebsiteTester.Parsers;
using WebsiteTester.Testers;

namespace WebsiteTester.Crawlers
{
    public class WebsiteCrawler
    {
        private readonly WebPageTester _webTester;
        private readonly PageCrawler _webCrawler;
        private readonly SitemapParser _siteMapParser;

        public WebsiteCrawler(SitemapParser siteMapParser, PageCrawler webCrawler, WebPageTester webPageTester)
        {
            _webTester = webPageTester;
            _webCrawler = webCrawler;
            _siteMapParser = siteMapParser;
        }

        public async Task<IEnumerable<WebLink>> GetUrls(string url)
        {
            var onPageUrls = _webCrawler.Crawl(url);
            var sitemapUrlps = _siteMapParser.Parse(url);

            var uniqueUrls = onPageUrls.Concat(sitemapUrlps)
                .GroupBy(x => x.Url)
                .Select(g =>
                    new WebLink()
                    {
                        Url = g.Key,
                        IsInSitemap = g.Any(x => x.IsInSitemap),
                        IsInWebsite = g.Any(x => x.IsInWebsite)
                    });

            return await _webTester.TestRenderTime(uniqueUrls);
        }
    }
}
