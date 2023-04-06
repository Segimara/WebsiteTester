using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WebsiteTester.Helpers;
using WebsiteTester.Services;
using WebsiteTester.Validators;

namespace WebsiteTester.Parsers
{
    public class WebsiteParser
    {
        private readonly UrlValidator _urlValidator;
        private readonly UrlNormalizer _urlNormalizer;
        private readonly ContentLoaderService _contentLoaderService;
        public WebsiteParser(UrlValidator urlValidator, UrlNormalizer urlNormalizer, ContentLoaderService contentLoaderService)
        {
            _urlValidator = urlValidator;
            _urlNormalizer = urlNormalizer;
            _contentLoaderService = contentLoaderService;
        }

        public IEnumerable<string> Parse(string url)
        {
            var doc = _contentLoaderService.Load(new Uri(url));

            if (!doc.DocumentNode.Descendants().Any())
            {
                throw new Exception("it is not a html document");
            }

            var validatedUrls = doc.DocumentNode.Descendants("a")
                .Select(a => a.GetAttributeValue("href", null))
                .Where(l => _urlValidator.IsValid(l));
            
            var normalizedUrls = _urlNormalizer.NormalizeUrls(validatedUrls, url);
            return normalizedUrls.Distinct();
        }
    }
}
