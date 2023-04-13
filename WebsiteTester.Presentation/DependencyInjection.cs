using Microsoft.Extensions.DependencyInjection;
using WebsiteTester.Presentation;

namespace WebsiteTester
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebsiteTesterPresentation(this IServiceCollection services)
        {
            services.AddTransient<ConsoleManager>();
            services.AddTransient<WebsiteTesterApplication>();

            return services;
        }
    }
}
