using WebsiteTester.Services;
using WebsiteTester.Services.Crawlers;
using WebsiteTester.Services.Parsers;
using WebsiteTester.Services.Validators;

namespace WebsiteTester.Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UrlValidator urlValidator = new UrlValidator();

            WebsiteParser parser = new WebsiteParser(urlValidator);
            SitemapParser siteMapParser = new SitemapParser(urlValidator);
            
            OnPageCrawler webCrawler = new OnPageCrawler(parser);
            PageLinkFinder linkFinder = new PageLinkFinder(siteMapParser, webCrawler);

            WebPageTester webTester = new WebPageTester();
            
            DomainCrawler domaincrawler = new DomainCrawler(linkFinder, webTester);

            WebsiteTesterApplication websiteTesterApplication = new WebsiteTesterApplication(domaincrawler);

            websiteTesterApplication.Run();
        }
    }
}