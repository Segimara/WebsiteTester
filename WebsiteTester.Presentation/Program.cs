using HtmlAgilityPack;
using WebsiteTester.Crawlers;
using WebsiteTester.Normalizers;
using WebsiteTester.Parsers;
using WebsiteTester.Services;
using WebsiteTester.Validators;

namespace WebsiteTester.Presentation
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            HttpClient httpClient = new HttpClient();
            ContentLoaderService contentLoaderService = new ContentLoaderService(htmlWeb);
            HttpClientService httpClientService = new HttpClientService(httpClient);

            UrlValidator urlValidator = new UrlValidator();
            UrlNormalizer urlNormalizer = new UrlNormalizer();

            WebsiteParser parser = new WebsiteParser(urlValidator, urlNormalizer, contentLoaderService);
            SitemapParser siteMapParser = new SitemapParser(urlValidator, urlNormalizer, httpClientService);
            TimeMeterService renderTimeMeter = new TimeMeterService(httpClientService);

            PageCrawler webCrawler = new PageCrawler(parser);

            WebsiteCrawler domainCrawler = new WebsiteCrawler(siteMapParser, webCrawler, renderTimeMeter);

            ConsoleManager console = new ConsoleManager();

            WebsiteTesterApplication websiteTesterApplication = new WebsiteTesterApplication(domainCrawler, console);

            await websiteTesterApplication.Run();
        }
    }
}