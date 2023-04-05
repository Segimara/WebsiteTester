using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteTester.Crawlers;
using WebsiteTester.Models;
using WebsiteTester.Services.Parsers;

namespace WebsiteTester.Services
{
    public class DomainLinkExtractor
    {
        private OnPageCrawler _webCrawler;
        private SitemapParser _siteMapParser;
        public DomainLinkExtractor(SitemapParser siteMapParser, OnPageCrawler webCrawler)
        {
            _siteMapParser = siteMapParser;
            _webCrawler = webCrawler;
        }

        public WebsiteLinksModel Extract(string url)
        {
            var onPageUrls = _webCrawler.Crawl(url).ToList();
            var sitemapUrlps = _siteMapParser.Parse(url).ToList();

            var onlyInSitemap = sitemapUrlps.Except(onPageUrls);
            var onlyInWebSite = onPageUrls.Except(sitemapUrlps);

            var uniqueUrls = onPageUrls.Concat(sitemapUrlps).Distinct();

            return new WebsiteLinksModel
            {
                LinksFromPages = onPageUrls,
                LinksFromSitemap = sitemapUrlps,
                LinksOnlyInSitemap = onlyInSitemap,
                LinksOnlyInWebsite = onlyInWebSite,
                UniqueLinks = uniqueUrls
            };
        }
    }
}
