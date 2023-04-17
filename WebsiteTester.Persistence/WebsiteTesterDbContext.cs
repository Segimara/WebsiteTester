using Microsoft.EntityFrameworkCore;
using WebsiteTester.Domain.Models;

namespace WebsiteTester.Persistenсe
{
    public class WebsiteTesterDbContext : DbContext
    {
        public DbSet<TestedLink> TestedLink { get; set; }
        public DbSet<LinkTestResult> LinkTestResult { get; set; }

        public WebsiteTesterDbContext(DbContextOptions<WebsiteTesterDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IEntityTypeConfiguration<>).Assembly);
        }
    }
}
