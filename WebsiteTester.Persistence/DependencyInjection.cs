using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebsiteTester.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebsiteTesterPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<WebsiteTesterDbContext>(options =>
                           //options.UseSqlServer(connectionString)
                           options.UseNpgsql(connectionString)
                           );

            return services;
        }
    }
}
