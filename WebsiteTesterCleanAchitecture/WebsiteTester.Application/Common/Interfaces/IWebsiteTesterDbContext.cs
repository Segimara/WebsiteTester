using WebsiteTester.Domain.Models;

namespace WebsiteTester.Application.Common.Interfaces
{
    public interface IWebsiteTesterDbContext
    {
        IQueryable<Link> Links { get; }
        IQueryable<LinkTestResult> LinkTestResults { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
