using Microsoft.EntityFrameworkCore;
using WebsiteTester.Common.Interfaces;
using WebsiteTester.Domain;
using WebsiteTester.Persistense.Configurations;

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
            modelBuilder.ApplyConfiguration(new TestedLinkConfiguration());
            modelBuilder.ApplyConfiguration(new LinkTestResultConfiguration());
        }
    }
}
