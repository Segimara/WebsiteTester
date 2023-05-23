using WebsiteTester.Application.Models;
using WebsiteTester.Application.WebsiteTester.Models;

namespace WebsiteTester.Application.WebsiteTester.Services
{
    public interface IResultsSaverService
    {
        Task<Result<bool>> GetAndSaveResultsAsync(string url);
        Task SaveResultsAsync(string testedUrl, IEnumerable<WebLink> testResults);
    }
}
