using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebsiteTester.Application.Common.Interfaces;
using WebsiteTester.Application.Features.WebsiteTester.Crawlers;
using WebsiteTester.Application.Features.WebsiteTester.Services;
using WebsiteTester.Application.Features.WebsiteTester.Validators;
using WebsiteTester.Application.Features.WebsiteTester.Validators.Interfaces;
using WebsiteTester.Crawler.Crawlers;
using WebsiteTester.Crawler.Normalizers;
using WebsiteTester.Crawler.Parsers;
using WebsiteTester.Crawler.Services;
using WebsiteTester.Crawler.Utility;
using WebsiteTester.Crawler.Validators;
using WebsiteTester.Crawler.Validators.Interfaces;
using WebsiteTester.Persistance;

namespace WebsiteTester.Infrastructure
{
    public static class DependencyContainer
    {
        public static void RegisterDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<WebsiteTesterDbContext>(options =>
                           options.UseSqlServer(connectionString)
                           );

            services.AddScoped<IWebsiteTesterDbContext, WebsiteTesterDbContext>();
        }

        public static void RegisterServices(this IServiceCollection services)
        {
            // Application Layer
            services.AddScoped<HtmlWeb>();
            services.AddScoped<HttpClient>();
            services.AddScoped<ISimpleUrlValidator, SimpleUrlValidator>();
            services.AddScoped<IComplexUrlValidator, ComplexUrlValidator>();
            services.AddScoped<IUrlNormalizer, UrlNormalizer>();

            services.AddScoped<HttpClientService, HttpClientService>();

            services.AddScoped<WebsiteParser>();
            services.AddScoped<SitemapParser>();
            services.AddScoped<TimeMeterUtility>();

            services.AddScoped<PageCrawler>();
            services.AddScoped<IWebsiteCrawler, WebsiteCrawler>();

            services.AddScoped<IResultsReceiverService, ResultsReceiverService>();
            services.AddScoped<IResultsSaverService, ResultsSaverService>();
        }

        public static void Migrate(this IServiceProvider serviceProvider)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<WebsiteTesterDbContext>();
                    context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetService<ILogger<WebsiteTesterDbContext>>();
                logger.LogError(ex, "An error occurred while migrating or initializing the database.");
            }
        }

    }
}
