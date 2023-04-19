using Microsoft.Extensions.DependencyInjection;
using WebsiteTester.MVC.Logic.Services;
using WebsiteTester.MVC.Logic.Validators;

namespace WebsiteTester.MVC.Logic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMvcLogic(this IServiceCollection services)
        {
            services.AddScoped<ResultsSaverService>();
            services.AddScoped<ResultsReceiverService>();
            services.AddTransient<UrlValidator>();

            return services;
        }
    }
}
