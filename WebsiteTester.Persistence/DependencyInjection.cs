using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebsiteTester.Common.Interfaces;

namespace WebsiteTester.Persistenсe
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebsiteTesterPersistenсe(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["DbConnection"];

            services.AddDbContext<WebsiteTesterDbContext>(options =>
                           options.UseSqlite(connectionString));

            services.AddScoped<IWebsiteTesterDbContext>(provider =>
                provider.GetService<WebsiteTesterDbContext>());

            return services;
        }
    }
}
