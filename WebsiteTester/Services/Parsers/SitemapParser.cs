using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
namespace WebsiteTester.Services.Parsers
{
    public class SitemapParser 
    {
        public IEnumerable<string> Parse(string baseUrl)
        {
            string sitemapXml = "";

            try
            {
                sitemapXml = GetSitemapXml(baseUrl).Result;
            }
            catch (Exception)
            {
                yield break;
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sitemapXml);

            var urlsXML = doc.GetElementsByTagName("url");

            var srtUrls = from XmlNode urlNode in urlsXML
                          select urlNode["loc"].InnerText;

            var correct = srtUrls.GetCorrectUrls(baseUrl);
            var unique = correct.Distinct();
             
            foreach (var url in unique)
            {
                yield return url;
            }

        }

        public async Task<string> GetSitemapXml(string url)
        {
            using (HttpClient client = new HttpClient())
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
