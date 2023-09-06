using WebsiteTester.Application.Features.WebsiteTester.DtoModels;
using WebsiteTester.Application.Models;

namespace WebsiteTester.Application.Features.WebsiteTester.Services
{
    public interface IResultsReceiverService
    {
        Task<IEnumerable<Link>> GetResultsAsync(Guid userId);
        Task<Result<Link>> GetTestDetailAsync(Guid userId, string id);
    }
}
