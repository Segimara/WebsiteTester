using Microsoft.EntityFrameworkCore;
using WebsiteTester.Application.Common.Interfaces;
using WebsiteTester.Domain.Models;
using WebsiteTester.Infrastructure.Persistence.Configurations;

namespace WebsiteTester.Infrastructure.Persistence
{
    public class WebsiteTesterDbContext : DbContext, IWebsiteTesterDbContext
    {
        public DbSet<Link> Links { get; set; }
        public DbSet<LinkTestResult> LinkTestResults { get; set; }

        public WebsiteTesterDbContext(DbContextOptions<WebsiteTesterDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LinkTestResultConfiguration).Assembly);
        }

        IQueryable<Link> IWebsiteTesterDbContext.Links => Links;
        IQueryable<LinkTestResult> IWebsiteTesterDbContext.LinkTestResults => LinkTestResults;

    }
}
