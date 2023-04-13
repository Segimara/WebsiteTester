using Autofac;
using Autofac.Extensions.DependencyInjection;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using WebsiteTester.Crawlers;
using WebsiteTester.Normalizers;
using WebsiteTester.Parsers;
using WebsiteTester.Services;
using WebsiteTester.Validators;

namespace WebsiteTester.Presentation
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.Populate(ConfigureServices());

            var servicesContainer = builder.Build();

            await servicesContainer.Resolve<WebsiteTesterApplication>().Run();
        }
        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();


            services.AddTransient<HtmlWeb>();
            services.AddTransient<HttpClient>();
            services.AddTransient<UrlValidator>();
            services.AddTransient<UrlNormalizer>();

            services.AddTransient<ContentLoaderService>();
            services.AddTransient<HttpClientService>();

            services.AddTransient<WebsiteParser>();
            services.AddTransient<SitemapParser>();
            services.AddTransient<TimeMeterService>();

            services.AddTransient<PageCrawler>();
            services.AddTransient<WebsiteCrawler>();

            services.AddTransient<ConsoleManager>();

            services.AddTransient<WebsiteTesterApplication>();

            return services;
        }
    }
}