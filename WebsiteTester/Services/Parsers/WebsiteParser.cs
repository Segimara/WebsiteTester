using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WebsiteTester.Services.Validators;

namespace WebsiteTester.Services.Parsers
{
    public class WebsiteParser
    {
        private readonly UrlValidator _urlValidator;

        public WebsiteParser(UrlValidator urlValidator)
        {
            _urlValidator = urlValidator;
        }

        public IEnumerable<string> Parse(string url)
        {
            var web = new HttpWebAgilityWrapper();
            var doc = web.Load(new Uri(url));

            if (!doc.DocumentNode.Descendants().Any())
            {
                throw new Exception("it is not a html document");
            }
            
            return doc.DocumentNode.Descendants("a")
                .Select(a => a.GetAttributeValue("href", null))
                .Where(l => _urlValidator.isValid(l))
                .NormalizeUrls(url)
                .Distinct();
        }
    }
}
