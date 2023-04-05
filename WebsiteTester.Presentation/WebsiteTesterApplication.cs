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
        private readonly DomainLinkExtractor _linkExtractor;
        private readonly WebPageTester _webTester;


        public WebsiteTesterApplication(DomainLinkExtractor linkExtractor, WebPageTester webPageTester)
        {
            _linkExtractor = linkExtractor;
            _webTester = webPageTester;
        }

        public void Run()
        {
            Console.WriteLine("Enter the website URL: ");
            string url = Console.ReadLine();
            StartTestUrl(url);
        }

        private void StartTestUrl(string url)
        {
            var linksFromUrl = _linkExtractor.Extract(url);

            OutputUrlsFromPage(linksFromUrl.LinksOnlyInWebsite, linksFromUrl.LinksOnlyInSitemap);

            var results = _webTester
                .Test(linksFromUrl.UniqueLinks)
                .OrderBy(x => x.Item2)
                .Select(result => $"{result.Item1} \t {result.Item2}");

            OutputList("Timing", results);

            Console.WriteLine($"Urls(html documents) found after crawling a website: {linksFromUrl.LinksFromPages.Count()}");
            Console.WriteLine($"Urls found in sitemap: {linksFromUrl.LinksFromSitemap.Count()}");
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
