using WebsiteTester.Models;
using WebsiteTester.Services.Crawlers;
using WebsiteTester.Services.Parsers;

namespace WebsiteTester.Services
{
    public class PageLinkFinder
    {
        private OnPageCrawler _webCrawler;
        private SitemapParser _siteMapParser;
        public PageLinkFinder(SitemapParser siteMapParser, OnPageCrawler webCrawler)
        {
            _siteMapParser = siteMapParser;
            _webCrawler = webCrawler;
        }

        public IEnumerable<WebLinkModel> Extract(string url)
        {
            var onPageUrls = _webCrawler.Crawl(url);
            var sitemapUrlps = _siteMapParser.Parse(url);

            var uniqueUrls = onPageUrls.Concat(sitemapUrlps)
                    .GroupBy(x => x.Url)
                    .Select(g =>
                        new WebLinkModel(g.Key,
                            g.Any(x => x.IsInSitemap),
                            g.Any(x => x.IsInWebsite)));

            return uniqueUrls;
        }

        //private IEnumerable<WebLinkModel> Concat(IEnumerable<WebLinkModel> fromWebsite, IEnumerable<WebLinkModel> fromSitemap)
        //{
        //    return null;
        //}
    }
}
