using WebsiteTester.Application.WebsiteTester.Models;

namespace WebsiteTester.Services;

public class TimeMeterService
{
    private readonly IHttpClientService _httpClientService;

    public TimeMeterService(IHttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }

    public virtual async Task<IEnumerable<WebLink>> TestRenderTimeAsync(IEnumerable<WebLink> urls)
    {
        List<WebLink> results = new List<WebLink>();

        foreach (var url in urls)
        {
            WebLink result = await SetRenderTimeAsync(url);
            results.Add(result);
        }

        return results;
    }

    private async Task<WebLink> SetRenderTimeAsync(WebLink url)
    {
        var uri = new Uri(url.Url);

        url.RenderTimeMilliseconds = _httpClientService.GetRenderTime(uri);

        return url;
    }
}