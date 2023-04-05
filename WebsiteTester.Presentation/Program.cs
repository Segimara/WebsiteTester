using WebsiteTester.Crawlers;
using WebsiteTester.Services;
using WebsiteTester.Services.Parsers;

namespace WebsiteTester.Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WebsiteParser parser = new WebsiteParser();
            SitemapParser siteMapParser = new SitemapParser();
            OnPageCrawler webCrawler = new OnPageCrawler(parser);
            WebPageTester webTester = new WebPageTester();
            WebsiteTesterApplication websiteTesterApplication = new WebsiteTesterApplication(siteMapParser, webCrawler, webTester);
        }
    }
}