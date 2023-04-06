using HtmlAgilityPack;
using WebsiteTester.Crawlers;
using WebsiteTester.Helpers;
using WebsiteTester.Parsers;
using WebsiteTester.Services;
using WebsiteTester.Validators;

namespace WebsiteTester.Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            ContentLoaderService contentLoaderService = new ContentLoaderService(htmlWeb);

            UrlValidator urlValidator = new UrlValidator();
            UrlNormalizer urlNormalizer = new UrlNormalizer();

            WebsiteParser parser = new WebsiteParser(urlValidator, urlNormalizer, contentLoaderService);
            SitemapParser siteMapParser = new SitemapParser(urlValidator, urlNormalizer, contentLoaderService);
            
            OnPageCrawler webCrawler = new OnPageCrawler(parser);

            WebPageTester webTester = new WebPageTester();
            
            DomainCrawler domainCrawler = new DomainCrawler(siteMapParser, webCrawler, webTester);

            WebsiteTesterApplication websiteTesterApplication = new WebsiteTesterApplication(domainCrawler);

            websiteTesterApplication.Run();
        }
    }
}