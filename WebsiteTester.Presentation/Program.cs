using Microsoft.Extensions.DependencyInjection;
using WebsiteTester.Persistence;

namespace WebsiteTester.Presentation
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddWebsiteTesterLogic();
            services.AddWebsiteTesterPresentation();

            services.AddWebsiteTesterPersistence(Environment.GetEnvironmentVariable("DB_CONNECTION"));

            var servicesProvider = services.BuildServiceProvider();

            await servicesProvider.GetService<WebsiteTesterApplication>().RunAsync();
        }
    }
}