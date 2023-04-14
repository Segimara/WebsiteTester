using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebsiteTester.Persistenсe;

namespace WebsiteTester.Presentation
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = GetConfig();

            var services = new ServiceCollection();

            services.AddWebsiteTesterLogic();
            services.AddWebsiteTesterPresentation();

            services.AddWebsiteTesterPersistenсe(configuration);

            var servicesProvider = services.BuildServiceProvider();

            await servicesProvider.GetService<WebsiteTesterApplication>().RunAsync();
        }

        private static IConfiguration GetConfig()
        {
            return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();
        }
    }
}