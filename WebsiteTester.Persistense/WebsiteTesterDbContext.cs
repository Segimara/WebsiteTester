using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebsiteTester.Common.Interfaces;
using WebsiteTester.Domain;

namespace WebsiteTester.Persistense
{
    public class WebsiteTesterDbContext : DbContext, IWebsiteTesterDbContext
    {
        public DbSet<TestedLink> TestedLink { get; set; }
        public DbSet<LinkTestResult> LinkTestResult { get; set; }

        public WebsiteTesterDbContext(DbContextOptions<WebsiteTesterDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public Task<int> SaveChangesAsync()
        {
            return this.SaveChangesAsync(CancellationToken.None);
        }
    }
}
