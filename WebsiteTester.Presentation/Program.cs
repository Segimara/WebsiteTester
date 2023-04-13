using Microsoft.Extensions.DependencyInjection;

namespace WebsiteTester.Presentation
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddWebsiteTesterLogic();
            services.AddWebsiteTesterPresentation();

            var servicesProvider = services.BuildServiceProvider();

            await servicesProvider.GetService<WebsiteTesterApplication>().RunAsync();
        }

    }
}