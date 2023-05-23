using WebsiteTester.Application.Models;
using WebsiteTester.Application.WebsiteTester.ViewModels;

namespace WebsiteTester.Application.WebsiteTester.Services
{
    public interface IResultsReceiverService
    {
        Task<IEnumerable<Link>> GetResultsAsync();
        Task<Result<Link>> GetTestDetailAsync(string id);
    }
}
