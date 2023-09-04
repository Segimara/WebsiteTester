using WebsiteTester.Application.Models;
using WebsiteTester.Domain.InternalModels;

namespace WebsiteTester.Application.Features.WebsiteTester.Services
{
    public interface IResultsSaverService
    {
        Task<Result<bool>> GetAndSaveResultsAsync(string url);
        Task SaveResultsAsync(string testedUrl, IEnumerable<WebLink> testResults);
    }
}
