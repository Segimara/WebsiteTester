using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebsiteTester.Persistance
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebsiteTesterPersistence(this IServiceCollection services, string connectionString = "Data Source=tester.db")
        {
            services.AddDbContext<WebsiteTesterDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}
