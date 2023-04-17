using Microsoft.EntityFrameworkCore;
using WebsiteTester.Domain.Models;
using WebsiteTester.Persistenсe.Configurations;

namespace WebsiteTester.Persistenсe
{
    public class WebsiteTesterDbContext : DbContext
    {
        public DbSet<TestedLink> TestedLinks { get; set; }
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
