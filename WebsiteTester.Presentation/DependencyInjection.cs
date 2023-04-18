using Microsoft.Extensions.DependencyInjection;
using WebsiteTester.Presentation;
using WebsiteTester.Services;

namespace WebsiteTester
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebsiteTesterPresentation(this IServiceCollection services)
        {
            services.AddTransient<ConsoleManager>();
            services.AddTransient<ResultsSaverService>();
            services.AddTransient<WebsiteTesterApplication>();

            return services;
        }
    }
}
