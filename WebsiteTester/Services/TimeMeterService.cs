using System.Diagnostics;
using WebsiteTester.Models;

namespace WebsiteTester.Services;

public class TimeMeterService
{
    private readonly HttpClientService _httpClientService;

    public TimeMeterService(HttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }

    public virtual async Task<IEnumerable<WebLink>> TestRenderTimeAsync(IEnumerable<WebLink> urls)
    {
        foreach (var url in urls)
        {
            await SetRenderTimeAsync(url);
        }

        return urls;
    }

    private async Task<WebLink> SetRenderTimeAsync(WebLink url)
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