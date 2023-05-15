using Microsoft.EntityFrameworkCore;
using WebsiteTester.Domain.Models;
using WebsiteTester.Persistence.Configurations;

namespace WebsiteTester.Persistence
{
    public class WebsiteTesterDbContext : DbContext
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
    }
}
