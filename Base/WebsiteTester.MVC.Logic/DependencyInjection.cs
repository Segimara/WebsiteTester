using Microsoft.Extensions.DependencyInjection;
using WebsiteTester.Web.Logic.Services;
using WebsiteTester.Web.Logic.Validators;

namespace WebsiteTester.Web.Logic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebLogic(this IServiceCollection services)
        {
            services.AddScoped<ResultsSaverService>();
            services.AddScoped<ResultsReceiverService>();
            services.AddTransient<UrlValidator>();

            return services;
        }
    }
}
