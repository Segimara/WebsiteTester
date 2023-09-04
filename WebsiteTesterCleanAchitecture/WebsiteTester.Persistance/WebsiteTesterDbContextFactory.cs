using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WebsiteTester.Persistance
{
    public class WebsiteTesterDbContectFactory : IDesignTimeDbContextFactory<WebsiteTesterDbContext>
    {
        public WebsiteTesterDbContext CreateDbContext(string[] args)
        {
            var dbContextBuilder = new DbContextOptionsBuilder<WebsiteTesterDbContext>();

            //var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
            var connectionString = "Data Source=tester.db";

            dbContextBuilder.UseSqlite(connectionString);

            return new WebsiteTesterDbContext(dbContextBuilder.Options);
        }
    }
}
