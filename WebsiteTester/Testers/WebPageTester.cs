using System.Diagnostics;
using WebsiteTester.Models;
using WebsiteTester.Services;

namespace WebsiteTester.Testers
{
    public class WebPageTester
    {
        private HttpClientService _httpClientService;

        public WebPageTester(HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<IEnumerable<WebLink>> TestRenderTime(IEnumerable<WebLink> urls)
        {
            return urls.Select(async url => await SetRenderTime(url)).Select( u => u.Result);
        }

        private async Task<WebLink> SetRenderTime(WebLink url)
        {
            var uri = new Uri(url.Url);
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            await _httpClientService.GetAsync(uri);

            stopwatch.Stop();

            url.RenderTimeMilliseconds = (int)stopwatch.ElapsedMilliseconds;

            return url;
        }
    }
}