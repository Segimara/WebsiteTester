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
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=WebsiteTesterDB;Trust Server Certificate=True;Encrypt=True;";

            dbContextBuilder.UseSqlServer(connectionString);

            return new WebsiteTesterDbContext(dbContextBuilder.Options);
        }
    }
}
