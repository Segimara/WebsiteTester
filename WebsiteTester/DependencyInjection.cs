using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using WebsiteTester.Crawlers;
using WebsiteTester.Normalizers;
using WebsiteTester.Parsers;
using WebsiteTester.Services;
using WebsiteTester.Validators;

namespace WebsiteTester
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebsiteTesterLogic(this IServiceCollection services)
        {
            services.AddTransient<HtmlWeb>();
            services.AddTransient<HttpClient>();
            services.AddTransient<UrlValidator>();
            services.AddTransient<UrlNormalizer>();

            services.AddTransient<ContentLoaderService>();
            services.AddTransient<HttpClientService>();

            services.AddTransient<WebsiteParser>();
            services.AddTransient<SitemapParser>();
            services.AddTransient<TimeMeterService>();
            services.AddTransient<ResultsSaverService>();

            services.AddTransient<PageCrawler>();
            services.AddTransient<WebsiteCrawler>();

            return services;
        }
    }
}
