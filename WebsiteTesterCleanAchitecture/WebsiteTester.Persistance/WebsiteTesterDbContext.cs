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

        void IWebsiteTesterDbContext.Add<TEntity>(TEntity entity)
        {
            this.Add(entity);
        }

        public void AddRange<TEntity>(IEnumerable<TEntity> entity)
        {
            this.AddRange(entity);
        }

        IQueryable<Link> IWebsiteTesterDbContext.Links => Links.AsQueryable<Link>();
        IQueryable<LinkTestResult> IWebsiteTesterDbContext.LinkTestResults => LinkTestResults.AsQueryable<LinkTestResult>();

    }
}
