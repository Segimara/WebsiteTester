using System.Diagnostics;
using WebsiteTester.Models;

namespace WebsiteTester.Services
{
    public class WebPageTester
    {
        private HttpClientService _httpClientService;

        public WebPageTester(HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }
        public IEnumerable<WebLinkModel> Test(IEnumerable<WebLinkModel> urls)
        {
            return urls
                .Select(url => GetRenderTime(url));
        }

        private WebLinkModel GetRenderTime(WebLinkModel url)
        {
            var uri = new Uri(url.Url);
            var stopwatch = new Stopwatch();
            
            stopwatch.Start();
            
            var response = _httpClientService.GetAsync(uri).Result;

            stopwatch.Stop();

            url.RenderTime = stopwatch.ElapsedMilliseconds;

            return url;
        }

    }
}