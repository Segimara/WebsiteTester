using WebsiteTester.Models;
using WebsiteTester.Parsers;
using WebsiteTester.Services;

namespace WebsiteTester.Crawlers
{
    public class DomainCrawler
    {
        private readonly WebPageTester _webTester;
        private readonly OnPageCrawler _webCrawler;
        private readonly SitemapParser _siteMapParser;
        public DomainCrawler(SitemapParser siteMapParser, OnPageCrawler webCrawler, WebPageTester webPageTester)
        {
            _webTester = webPageTester;
            _webCrawler = webCrawler;
            _siteMapParser = siteMapParser;
        }

        public IEnumerable<WebLinkModel> GetUrls(string url)
        {
            var onPageUrls = _webCrawler.Crawl(url);
            var sitemapUrlps = _siteMapParser.Parse(url);

            var uniqueUrls = onPageUrls.Concat(sitemapUrlps)
                .GroupBy(x => x.Url)
                .Select(g =>
                    new WebLinkModel()
                    {
                        Url = g.Key,
                        IsInSitemap = g.Any(x => x.IsInSitemap),
                        IsInWebsite = g.Any(x => x.IsInWebsite)
                    });

            return _webTester.Test(uniqueUrls);
        }
    }
}
