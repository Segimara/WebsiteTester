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
            DomainLinkExtractor linkExtractor = new DomainLinkExtractor(siteMapParser, webCrawler);

            WebPageTester webTester = new WebPageTester();
            
            WebsiteTesterApplication websiteTesterApplication = new WebsiteTesterApplication(linkExtractor, webTester);

            websiteTesterApplication.Run();
        }
    }
}