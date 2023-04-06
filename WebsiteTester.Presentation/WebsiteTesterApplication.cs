using WebsiteTester.Models;
using WebsiteTester.Services;

namespace WebsiteTester.Presentation
{
    public class WebsiteTesterApplication
    {
        private readonly DomainCrawler _crawler;

        public WebsiteTesterApplication(DomainCrawler crawler)
        {
            _crawler = crawler;
        }

        public void Run()
        {
            Console.WriteLine("Enter the website URL: ");
            string url = Console.ReadLine();
            GetResults(url);
        }

        private void GetResults(string url)
        {
            var linksFromUrl = _crawler.GetUrls(url).ToList();

            var onlyInWebSite = linksFromUrl.Where(l => l is { IsInWebsite: true, IsInSitemap: false });
            var onlyInSitemap = linksFromUrl.Where(l => l is { IsInSitemap: true, IsInWebsite: true });
            
            OutputUrlsFromPage(onlyInWebSite, onlyInSitemap);

            var results = linksFromUrl
                .OrderBy(x => x.RenderTime)
                .Select(result => $"{result.Url} \t {result.RenderTime}");

            OutputList("Timing", results);

            Console.WriteLine($"Urls(html documents) found after crawling a website: {onlyInWebSite.Count()}");
            Console.WriteLine($"Urls found in sitemap: {onlyInSitemap.Count()}");
        }

        private void OutputUrlsFromPage(IEnumerable<WebLinkModel> onlyInWebSite, IEnumerable<WebLinkModel> onlyInSitemap)
        {
            string messageForUrlsInSitemap = "Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site";
            OutputList(messageForUrlsInSitemap, onlyInSitemap.Select(u => u.Url));
            string messageForUrlsInWebSite = "Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml";
            OutputList(messageForUrlsInWebSite, onlyInWebSite.Select(u => u.Url));
        }

        private void OutputList(string preMessage, IEnumerable<string> urls)
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
