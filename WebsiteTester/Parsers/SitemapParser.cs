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
        private ContentLoaderService _contentLoaderService;

        public SitemapParser(UrlValidator urlValidator, UrlNormalizer urlNormalizer, ContentLoaderService contentLoaderService)
        {
            _urlValidator = urlValidator;
            _urlNormalizer = urlNormalizer;
            _contentLoaderService = contentLoaderService;
        }

        public IEnumerable<WebLinkModel> Parse(string baseUrl)
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

            var validatedUrls = urlsList
                .Where(l => _urlValidator.IsValid(l));
            var normalizedUrls = _urlNormalizer.NormalizeUrls(validatedUrls, baseUrl);

            return normalizedUrls
                .Distinct()
                .Select(u => new WebLinkModel()
                {
                    Url = u,
                    IsInSitemap = true,
                    IsInWebsite = false
                });
        }

        public async Task<string> GetSitemapXml(string url)
        {
            try
            {
                string content = _contentLoaderService.Load(new Uri(new Uri(url), "/sitemap.xml"))
                    .Text;
                return content;
            }
            catch (Exception e)
            {
                throw;
            }


        }
    }
}
