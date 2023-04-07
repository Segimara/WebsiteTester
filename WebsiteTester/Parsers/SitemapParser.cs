using System.Xml;
using WebsiteTester.Helpers;
using WebsiteTester.Models;
using WebsiteTester.Services;
using WebsiteTester.Validators;

namespace WebsiteTester.Parsers
{
    public class SitemapParser
    {
        private readonly UrlValidator _urlValidator;
        private readonly UrlNormalizer _urlNormalizer;
        private readonly HttpClientService _httpClientService;

        public SitemapParser(UrlValidator urlValidator, UrlNormalizer urlNormalizer, HttpClientService httpClientService)
        {
            _urlValidator = urlValidator;
            _urlNormalizer = urlNormalizer;
            _httpClientService = httpClientService;
        }

        public IEnumerable<WebLink> Parse(string baseUrl)
        {
            string sitemapContent = "";

            try
            {
                sitemapContent = GetSitemapXml(baseUrl).Result;
            }
            catch (Exception)
            {
                return null;
            }

            XmlDocument sitemapDoc = new XmlDocument();
            sitemapDoc.LoadXml(sitemapContent);

            var urlsNodes = sitemapDoc.GetElementsByTagName("url");

            var urlsList = urlsNodes.Cast<XmlNode>()
                .Select(urlNode => urlNode["loc"].InnerText);

            var validatedUrls = urlsList.Where(l => _urlValidator.IsValid(l));

            var normalizedUrls = _urlNormalizer.NormalizeUrls(validatedUrls, baseUrl);

            return normalizedUrls
                .Distinct()
                .Select(u => new WebLink()
                {
                    Url = u,
                    IsInSitemap = true,
                    IsInWebsite = false
                });
        }

        public async Task<string> GetSitemapXml(string url)
        {
            var response = await _httpClientService.GetAsync(new Uri(new Uri(url), "/sitemap.xml"));

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return content;
            }

            throw new Exception($"Failed to retrieve sitemap.xml from {url}. StatusCode: {response.StatusCode}");
        }
    }
}
