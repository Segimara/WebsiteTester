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
            HttpClient httpClient = new HttpClient();
            ContentLoaderService contentLoaderService = new ContentLoaderService(htmlWeb);
            HttpClientService httpClientService = new HttpClientService(httpClient);

            UrlValidator urlValidator = new UrlValidator();
            UrlNormalizer urlNormalizer = new UrlNormalizer();

            WebsiteParser parser = new WebsiteParser(urlValidator, urlNormalizer, contentLoaderService);
            SitemapParser siteMapParser = new SitemapParser(urlValidator, urlNormalizer, httpClientService);
            WebPageTester webTester = new WebPageTester(httpClientService);
            
            OnPageCrawler webCrawler = new OnPageCrawler(parser);

            DomainCrawler domainCrawler = new DomainCrawler(siteMapParser, webCrawler, webTester);

            WebsiteTesterApplication websiteTesterApplication = new WebsiteTesterApplication(domainCrawler);

            websiteTesterApplication.Run();
        }
    }
}