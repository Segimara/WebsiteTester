using WebsiteTester.Application.Features.WebsiteTester.Crawlers;
using WebsiteTester.Application.Features.WebsiteTester.Services;
using WebsiteTester.Domain.InternalModels;

namespace WebsiteTester.Presentation
{
    public class WebsiteTesterApplication
    {
        private readonly IWebsiteCrawler _crawler;
        private readonly ConsoleManager _console;
        private readonly IResultsSaverService _resultsSaver;

        public WebsiteTesterApplication(IWebsiteCrawler crawler, ConsoleManager console, IResultsSaverService resultsSaver)
        {
            _crawler = crawler;
            _console = console;
            _resultsSaver = resultsSaver;
        }

        public async Task RunAsync()
        {
            _console.WriteLine("Enter the website URL: ");

            var url = _console.ReadLine();

            await GetResultsAsync(url);

        }

        private async Task GetResultsAsync(string url)
        {
            var linksFromUrl = await _crawler.GetUrlsAsync(url);

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

            _console.WriteLine($"Urls(html documents) found after crawling a website: {linksFromUrl.Count(u => u.IsInWebsite)}");
            _console.WriteLine($"Urls found in sitemap: {linksFromUrl.Count(u => u.IsInSitemap)}");

            await _resultsSaver.SaveResultsAsync(url, linksFromUrl);
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
            var i = 1;

            _console.WriteLine(message);

            foreach (var u in urls)
            {
                _console.WriteLine($"{i++}) {u}");
            }
        }
    }
}
