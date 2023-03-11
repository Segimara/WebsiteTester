using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WebsiteTester.Interfaces;

namespace WebsiteTester.Crawlers
{
    //Separate from the IPageCrawler interface because you can get sitemap in different URLs and in different formats 
    public class SitemapParser : ISiteMapParser
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
            foreach (var url in srtUrls)
            {
                yield return url;
            }

        }
        public async Task<string> GetSitemapXml(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url + "/sitemap.xml");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                else
                {
                    throw new Exception($"Failed to retrieve sitemap.xml from {url}. StatusCode: {response.StatusCode}");
                }
            }
        }
    }
}
