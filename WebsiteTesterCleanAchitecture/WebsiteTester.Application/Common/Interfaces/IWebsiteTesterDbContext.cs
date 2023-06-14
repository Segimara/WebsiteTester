using WebsiteTester.Domain.Models;

namespace WebsiteTester.Application.Common.Interfaces
{
    public interface IWebsiteTesterDbContext
    {
        IQueryable<Link> Links { get; }
        IQueryable<LinkTestResult> LinkTestResults { get; }

        void Add<TEntity>(TEntity entity);
        void AddRange<TEntity>(IEnumerable<TEntity> entity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
