using WebsiteTester.Crawler.Models;

namespace WebsiteTester.Application.WebsiteTester.Crawlers
{
    public interface IWebsiteCrawler
    {
        Task<IEnumerable<WebLink>> GetUrlsAsync(string url);
    }
}
