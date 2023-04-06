using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WebsiteTester.Models;
using WebsiteTester.Services.Validators;

namespace WebsiteTester.Services.Parsers
{
    public class SitemapParser 
    {
        private readonly UrlValidator _urlValidator;

        public SitemapParser(UrlValidator urlValidator)
        {
            _urlValidator = urlValidator;
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

            var normalizedUrls = urlsList
                .Where(l => _urlValidator.isValid(l))
                .NormalizeUrls(baseUrl);

            return normalizedUrls
                .Distinct()
                .Select(u => new WebLinkModel(u, true, false));
        }

        public async Task<string> GetSitemapXml(string url)
        {
            using (HttpClientWrapper client = new HttpClientWrapper())
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(new Uri(url), "/sitemap.xml"));

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return content;
                }

                throw new Exception($"Failed to retrieve sitemap.xml from {url}. StatusCode: {response.StatusCode}");
            }
        }
    }
}
