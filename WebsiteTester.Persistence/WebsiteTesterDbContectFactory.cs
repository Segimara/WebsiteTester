using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WebsiteTester.Persistenсe
{
    public class WebsiteTesterDbContectFactory : IDesignTimeDbContextFactory<WebsiteTesterDbContext>
    {
        public WebsiteTesterDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var dbContextBuilder = new DbContextOptionsBuilder<WebsiteTesterDbContext>();

            //todo understand how to load config from other project
            var connectionString = "Data Source = WebsiteTester.db;";

            dbContextBuilder.UseSqlite(connectionString);

            return new WebsiteTesterDbContext(dbContextBuilder.Options);
        }
    }
}
