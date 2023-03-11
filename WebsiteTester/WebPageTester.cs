using System.Diagnostics;
using System.Net;
using WebsiteTester.Interfaces;

namespace WebsiteTester
{
    public class WebPageTester : IPageTester
    {
        private IPageCrawler _pageCrawler;
        private ISiteMapParser _siteMapParser;
        public WebPageTester(IPageCrawler crawler, ISiteMapParser siteMapParser)
        {
            _pageCrawler = crawler;
            _siteMapParser = siteMapParser;
        }
        public void Test(string url)
        {
            var onPageUrls = _pageCrawler.Crawl(url);
            var sitemapUrlps = _siteMapParser.Parse(url);
            var onlyInSitemap = sitemapUrlps.Except(onPageUrls);
            var onlyInWebSite = onPageUrls.Except(sitemapUrlps);
            var uniqueUrls = onPageUrls.Concat(onlyInSitemap);
            var results = TestUrls(uniqueUrls).OrderBy(x => x.Item2);
            // i can create nice "table" output but don`t think it's really necessary in this task
            string messageForUrlsInSitemap = "Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site";
            OutputUrls(messageForUrlsInSitemap, onlyInSitemap);
            string messageForUrlsInWebSite = "Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml";
            OutputUrls(messageForUrlsInSitemap, onlyInWebSite);
            int i = 1;
            Console.WriteLine("Timing");
            foreach (var u in results)
            {
                Console.WriteLine($"{i++}) {u.Item1} \t {u.Item2}");
            }
            Console.WriteLine($"Urls(html documents) found after crawling a website: {onlyInWebSite.Count()}");
            Console.WriteLine($"Urls found in sitemap: {onlyInSitemap.Count()}");
        }
        private IEnumerable<(string, long)> TestUrls(IEnumerable<string> urls)
        {
            // test every url and return the response time+url using yeild return
            foreach (var url in urls)
            {
                yield return (url, GetRenderTime(url));
            }
        }
        
        private long GetRenderTime(string url)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
            }
            catch (Exception)
            {
                //do i need catch specific status codes?
            }
            finally
            {
                client.Dispose();
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        private void OutputUrls(string preMessage, IEnumerable<string> urls)
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