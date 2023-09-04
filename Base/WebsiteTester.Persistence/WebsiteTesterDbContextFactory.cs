using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WebsiteTester.Persistence
{
    public class WebsiteTesterDbContectFactory : IDesignTimeDbContextFactory<WebsiteTesterDbContext>
    {
        public WebsiteTesterDbContext CreateDbContext(string[] args)
        {
            var dbContextBuilder = new DbContextOptionsBuilder<WebsiteTesterDbContext>();

            //var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=WebsiteTesterDB;Trusted_Connection=True;";

            dbContextBuilder.UseSqlServer(connectionString);

            return new WebsiteTesterDbContext(dbContextBuilder.Options);
        }
    }
}
