using Microsoft.EntityFrameworkCore;
using WebsiteTester.Persistenсe;

namespace WebsiteTester.Presentation.Tests
{
    public class WebsiteTesterContextFactory
    {
        public static WebsiteTesterDbContext Create()
        {
            var options = new DbContextOptionsBuilder<WebsiteTesterDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new WebsiteTesterDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }
        public static void Destroy(WebsiteTesterDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
