using HtmlAgilityPack;
using WebsiteTester.Crawlers;
using WebsiteTester.Normalizers;
using WebsiteTester.Parsers;
using WebsiteTester.Services;
using WebsiteTester.Validators;
using WebsiteTester.Wrappers;

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
            PageRenderTimeMeterService renderTimeMeter = new PageRenderTimeMeterService(httpClientService);
            
            PageCrawler webCrawler = new PageCrawler(parser);

            WebsiteCrawler domainCrawler = new WebsiteCrawler(siteMapParser, webCrawler, renderTimeMeter);

            ConsoleWrapper console = new ConsoleWrapper();

            WebsiteTesterApplication websiteTesterApplication = new WebsiteTesterApplication(domainCrawler, console);

            websiteTesterApplication.Run();
        }
    }
}