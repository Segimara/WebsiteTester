using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using WebsiteTester.Normalizers;
using WebsiteTester.Parsers;
using WebsiteTester.Services;
using WebsiteTester.Validators;
using Xunit;

namespace WebsiteTester.Tests
{
    public class SitemapParserTests
    {
        

        public SitemapParserTests()
        {
            HttpClient httpClient = new HttpClient();

            var httpClientServiceMoq = new Mock<HttpClientService>(httpClient);
            
            var urlNormalizer = new Mock<UrlNormalizer>();
            var urlValidator = new Mock<UrlValidator>();

            var sitemapParser = new SitemapParser(urlValidator.Object, urlNormalizer.Object, httpClientServiceMoq.Object);
        }

        [Fact]
        public void Parse_WebsiteWithoutSitemap_ShouldReturnNull()
        {

        }
    }
}
