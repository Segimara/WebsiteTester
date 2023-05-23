﻿using Microsoft.Extensions.Logging;
using System.Xml;
using WebsiteTester.Application.Normalizers;
using WebsiteTester.Application.WebsiteTester.Models;
using WebsiteTester.Application.WebsiteTester.Validators.Interfaces;
using WebsiteTester.Services;

namespace WebsiteTester.Parsers;

public class SitemapParser
{
    private readonly IHttpClientService _httpClientService;
    private readonly ILogger _logger;
    private readonly IUrlNormalizer _urlNormalizer;
    private readonly ISimpleUrlValidator _urlValidator;

    public SitemapParser(ISimpleUrlValidator urlValidator, IUrlNormalizer urlNormalizer, IHttpClientService httpClientService, ILogger<SitemapParser> logger)
    {
        _urlValidator = urlValidator;
        _urlNormalizer = urlNormalizer;
        _httpClientService = httpClientService;
        _logger = logger;
    }

    public virtual async Task<IEnumerable<WebLink>> ParseAsync(string baseUrl)
    {
        var sitemapContent = "";

        var baseUri = new Uri(baseUrl);

        try
        {
            sitemapContent = await GetSitemapXml(baseUri);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex.Message);

            return Enumerable.Empty<WebLink>();
        }

        var sitemapDoc = new XmlDocument();
        sitemapDoc.LoadXml(sitemapContent);

        var urlsNodes = sitemapDoc.GetElementsByTagName("url");

        var urlsList = urlsNodes.Cast<XmlNode>()
            .Select(urlNode => urlNode["loc"].InnerText);

        var validatedUrls = urlsList.Where(l => _urlValidator.IsValid(l));

        var normalizedUrls = _urlNormalizer.NormalizeUrls(validatedUrls, baseUrl);

        return normalizedUrls
            .Distinct()
            .Select(u => new WebLink
            {
                Url = u,
                IsInSitemap = true,
                IsInWebsite = false
            });
    }

    public async Task<string> GetSitemapXml(Uri url)
    {
        var response = _httpClientService.GetContent(new Uri(url, "/sitemap.xml"));

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            return content;
        }

        throw new Exception($"Failed to retrieve sitemap.xml from {url}. StatusCode: {response.StatusCode}");
    }
}
