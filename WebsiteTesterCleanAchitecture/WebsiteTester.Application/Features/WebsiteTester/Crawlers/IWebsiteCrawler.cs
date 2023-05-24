using WebsiteTester.Domain.Models;

namespace WebsiteTester.Application.Features.WebsiteTester.Crawlers
{
    public interface IWebsiteCrawler
    {
        Task<IEnumerable<WebLink>> GetUrlsAsync(string url);
    }
}
