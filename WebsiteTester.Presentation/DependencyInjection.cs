using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebsiteTester.Crawlers;
using WebsiteTester.Parsers;
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

            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                .AddConsole()
                .SetMinimumLevel(LogLevel.Information);
            });

            services.AddSingleton(loggerFactory.CreateLogger<SitemapParser>());
            services.AddSingleton(loggerFactory.CreateLogger<PageCrawler>());
            return services;
        }
    }
}