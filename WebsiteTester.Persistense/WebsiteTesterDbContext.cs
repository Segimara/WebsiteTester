using Microsoft.EntityFrameworkCore;

namespace WebsiteTester.Persistense
{
    public class WebsiteTesterDbContext : DbContext
    {
        public DbSet<TestedLink>
    }
}
