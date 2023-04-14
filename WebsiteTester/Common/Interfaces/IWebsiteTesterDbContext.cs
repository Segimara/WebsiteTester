using Microsoft.EntityFrameworkCore;
using WebsiteTester.Domain;

namespace WebsiteTester.Common.Interfaces
{
    public interface IWebsiteTesterDbContext
    {
        DbSet<TestedLink> TestedLink { get; set; }
        DbSet<LinkTestResult> LinkTestResult { get; set; }

        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
