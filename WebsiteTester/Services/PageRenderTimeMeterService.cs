using System.Diagnostics;
using WebsiteTester.Models;

namespace WebsiteTester.Services;

public class PageRenderTimeMeterService
{
    private readonly HttpClientService _httpClientService;

    public PageRenderTimeMeterService(HttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }

    public async Task<IEnumerable<WebLink>> TestRenderTime(IEnumerable<WebLink> urls)
    {
        return urls.Select(async url => await SetRenderTime(url)).Select(u => u.Result);
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