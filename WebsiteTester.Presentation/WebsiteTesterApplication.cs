using WebsiteTester.Crawlers;
using WebsiteTester.Models;

namespace WebsiteTester.Presentation
{
    public class WebsiteTesterApplication
    {
        private readonly WebsiteCrawler _crawler;

        public WebsiteTesterApplication(WebsiteCrawler crawler)
        {
            _crawler = crawler;
        }

        public void Run()
        {
            Console.WriteLine("Enter the website URL: ");

            var url = Console.ReadLine();
            
            GetResults(url);
        }

        private async void GetResults(string url)
        {
            var linksFromUrl = (await _crawler.GetUrls(url)).ToList();

            var onlyInWebSite = linksFromUrl
                .Where(l => l.IsInWebsite)
                .Where(l => !l.IsInSitemap);

            var onlyInSitemap = linksFromUrl
                .Where(l => l.IsInSitemap)
                .Where(l => !l.IsInWebsite);
            
            OutputUrlsFromPage(onlyInWebSite, onlyInSitemap);

            var results = linksFromUrl
                .OrderBy(x => x.RenderTimeMilliseconds)
                .Select(result => $"{result.Url} \t {result.RenderTimeMilliseconds}");

            OutputList("Timing", results);

            Console.WriteLine($"Urls(html documents) found after crawling a website: {onlyInWebSite.Count()}");
            Console.WriteLine($"Urls found in sitemap: {onlyInSitemap.Count()}");
        }

        private void OutputUrlsFromPage(IEnumerable<WebLink> onlyInWebSite, IEnumerable<WebLink> onlyInSitemap)
        {
            var messageForUrlsInSitemap = "Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site";
            OutputList(messageForUrlsInSitemap, onlyInSitemap.Select(u => u.Url));

            var messageForUrlsInWebSite = "Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml";
            OutputList(messageForUrlsInWebSite, onlyInWebSite.Select(u => u.Url));
        }

        private void OutputList(string message, IEnumerable<string> urls)
        {
            int i = 1;
            
            Console.WriteLine(message);

            foreach (var u in urls)
            {
                Console.WriteLine($"{i++}) {u}");
            }
        }
    }
}
