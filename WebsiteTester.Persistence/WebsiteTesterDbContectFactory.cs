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
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;";

            dbContextBuilder.UseSqlServer(connectionString);

            return new WebsiteTesterDbContext(dbContextBuilder.Options);
        }
    }
}
