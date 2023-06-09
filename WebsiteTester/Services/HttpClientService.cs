﻿namespace WebsiteTester.Services;

public class HttpClientService
{
    private readonly HttpClient _webClient;

    public HttpClientService(HttpClient webClient)
    {
        _webClient = webClient;
    }

    public virtual async Task<HttpResponseMessage> GetAsync(Uri url)
    {
        return await _webClient.GetAsync(url);
    }
}