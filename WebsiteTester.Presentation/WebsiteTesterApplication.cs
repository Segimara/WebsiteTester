using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteTester.Crawlers;
using WebsiteTester.Services;
using WebsiteTester.Services.Parsers;

namespace WebsiteTester.Presentation
{
    public class WebsiteTesterApplication
    {
        private readonly OnPageCrawler _webCrawler;
        private readonly SitemapParser _siteMapParser;
        private readonly WebPageTester _webTester;


        public WebsiteTesterApplication(SitemapParser siteMapParser, OnPageCrawler webCrawler, WebPageTester webPageTester)
        {
            _siteMapParser = siteMapParser;
            _webCrawler = webCrawler;   
            _webTester = webPageTester;
        }

        public void Run()
        {
            Console.WriteLine("Enter the website URL: ");
            string url = Console.ReadLine();
            StartTestUrl(url);
        }

        private void StartTestUrl(string? url)
        {
            var onPageUrls = _webCrawler.Crawl(url).ToList();
            var sitemapUrlps = _siteMapParser.Parse(url).ToList();

            var onlyInSitemap = sitemapUrlps.Except(onPageUrls);
            var onlyInWebSite = onPageUrls.Except(sitemapUrlps);

            var uniqueUrls = onPageUrls.Concat(sitemapUrlps).Distinct();

            OutputUrlsFromPage(onlyInWebSite, onlyInSitemap);

            var results = _webTester
                .Test(uniqueUrls)
                .OrderBy(x => x.Item2)
                .Select(result => $"{result.Item1} \t {result.Item2}");

            OutputList("Timing", results);

            Console.WriteLine($"Urls(html documents) found after crawling a website: {onPageUrls.Count()}");
            Console.WriteLine($"Urls found in sitemap: {sitemapUrlps.Count()}");
        }

        private void OutputUrlsFromPage(IEnumerable<string> onlyInWebSite, IEnumerable<string> onlyInSitemap)
        {
            string messageForUrlsInSitemap = "Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site";
            OutputList(messageForUrlsInSitemap, onlyInSitemap);
            string messageForUrlsInWebSite = "Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml";
            OutputList(messageForUrlsInWebSite, onlyInWebSite);
        }

        private void OutputList(string preMessage, IEnumerable<string> urls)
        {
            int i = 1;
            Console.WriteLine(preMessage);
            foreach (var u in urls)
            {
                Console.WriteLine($"{i++}) {u}");
            }
        }
    }
}
