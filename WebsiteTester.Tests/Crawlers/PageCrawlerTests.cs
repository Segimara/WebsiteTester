using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteTester.Crawlers;
using WebsiteTester.Normalizers;
using WebsiteTester.Parsers;
using WebsiteTester.Services;
using WebsiteTester.Validators;
using Xunit;

namespace WebsiteTester.Tests.Crawlers
{
    public class PageCrawlerTests
    {
        private readonly Mock<ContentLoaderService> _httpClientService;
        private readonly Mock<UrlNormalizer> _urlNormalizer;
        private readonly Mock<UrlValidator> _urlValidator;
        private readonly Mock<WebsiteParser> _websiteParser;

        private readonly PageCrawler _pageCrawler;

        public PageCrawlerTests()
        {
            var httpClient = new HttpClient();

            _httpClientServiceMoq = new Mock<HttpClientService>(httpClient);
            _urlNormalizer = new Mock<UrlNormalizer>();
            _urlValidator = new Mock<UrlValidator>();

            _pageCrawler = new PageCrawler();
        }

        [Fact]
        public void Crawl_WhenUrlIsNull_ShouldReturnEmptyList()
        {
            var result = _pageCrawler.Crawl(null);
            Assert.Empty(result);
        }

        [Fact]
        public void Crawl_WhenUrlIsEmpty_ShouldReturnEmptyList()
        {
            var result = _pageCrawler.Crawl(string.Empty);
            Assert.Empty(result);
        }

        [Fact]
        public void Crawl_WhenUrlIsNotValid_ShouldReturnEmptyList()
        {
            var result = _pageCrawler.Crawl("skype:someSkype");
            Assert.Empty(result);
        }

        [Fact]
        public void Crawl_WhenUrlIsValid_ShouldReturnListOfUrls()
        {
            var result = _pageCrawler.Crawl("");
            Assert.NotEmpty(result);
        }
    }
}
