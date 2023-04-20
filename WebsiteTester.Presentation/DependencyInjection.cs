using Microsoft.Extensions.DependencyInjection;
using WebsiteTester.Presentation.Services;

namespace WebsiteTester.Presentation
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
