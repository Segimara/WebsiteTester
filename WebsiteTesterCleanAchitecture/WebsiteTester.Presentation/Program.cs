using Microsoft.Extensions.DependencyInjection;
using WebsiteTester.Infrastructure;

namespace WebsiteTester.Presentation
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddWebsiteTesterPresentation();
            services.RegisterServices();
            services.RegisterDbContext(Environment.GetEnvironmentVariable("DB_CONNECTION"));

            var servicesProvider = services.BuildServiceProvider();

            await servicesProvider.GetService<WebsiteTesterApplication>().RunAsync();
        }
    }
}