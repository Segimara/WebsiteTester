using WebsiteTester.Crawlers;
using WebsiteTester.Interfaces;

namespace WebsiteTester
{
    public class Program
    {
        static void Main(string[] args)
        {
            IPageCrawler webCrawler = new OnPageCrawler();
            ISiteMapParser siteMapParser = new SitemapParser();
            WebPageTester webTester = new WebPageTester(webCrawler, siteMapParser);
            Console.WriteLine("Enter the website URL: ");
            string url = Console.ReadLine();
            webTester.Test(url);
        }
    }
}