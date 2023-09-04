using Microsoft.EntityFrameworkCore;
using WebsiteTester.Application.Common.Interfaces;
using WebsiteTester.Domain.Models;
using WebsiteTester.Persistance.Configurations;

namespace WebsiteTester.Persistance
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

        public void Add<T>(T link) where T : class
        {
            base.Add(link);
        }

        public void AddRange<T>(IEnumerable<T> links) where T : class
        {
            base.AddRange(links);
        }

    }
}
