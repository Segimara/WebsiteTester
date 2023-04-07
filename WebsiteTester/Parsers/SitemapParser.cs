using System.Xml;
using WebsiteTester.Models;
using WebsiteTester.Normalizers;
using WebsiteTester.Services;
using WebsiteTester.Validators;

namespace WebsiteTester.Parsers;

public class SitemapParser
{
    private readonly HttpClientService _httpClientService;
    private readonly UrlNormalizer _urlNormalizer;
    private readonly UrlValidator _urlValidator;

    public SitemapParser(UrlValidator urlValidator, UrlNormalizer urlNormalizer, HttpClientService httpClientService)
    {
        _urlValidator = urlValidator;
        _urlNormalizer = urlNormalizer;
        _httpClientService = httpClientService;
    }

    public IEnumerable<WebLink> Parse(string baseUrl)
    {
        var sitemapContent = "";

        var baseUri = new Uri(baseUrl);

        try
        {
            sitemapContent = GetSitemapXml(baseUri).Result;
        }
        catch (Exception)
        {
            return null;
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
        var response = await _httpClientService.GetAsync(new Uri(url, "/sitemap.xml"));

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            return content;
        }

        throw new Exception($"Failed to retrieve sitemap.xml from {url}. StatusCode: {response.StatusCode}");
    }
}