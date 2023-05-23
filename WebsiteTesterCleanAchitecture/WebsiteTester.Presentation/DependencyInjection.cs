using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebsiteTester.Application.WebsiteTester.Crawlers;
using WebsiteTester.Application.WebsiteTester.Parsers;

namespace WebsiteTester.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebsiteTesterPresentation(this IServiceCollection services)
        {
            services.AddTransient<ConsoleManager>();
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