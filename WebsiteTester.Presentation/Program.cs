using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebsiteTester.Persistense;
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

            services.AddWebsiteTesterPersistense(configuration);

            var servicesProvider = services.BuildServiceProvider();

            await servicesProvider.GetService<WebsiteTesterApplication>().RunAsync();
        }

        private static IConfiguration GetConfig()
        {
            return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"DbConnection", $"Data Source = WebsiteTester.db"}
            })
            .Build();
        }
    }
}