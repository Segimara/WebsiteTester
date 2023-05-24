using Microsoft.Extensions.Logging;
using Moq;
using WebsiteTester.Crawler.Crawlers;
using WebsiteTester.Crawler.Normalizers;
using WebsiteTester.Crawler.Parsers;
using WebsiteTester.Crawler.Validators.Interfaces;
using Xunit;

namespace WebsiteTester.Tests.Crawlers
{
    public class PageCrawlerTests
    {
        private readonly Mock<IHttpClientService> _contentLoaderService;
        private readonly Mock<IUrlNormalizer> _urlNormalizer;
        private readonly Mock<ISimpleUrlValidator> _urlValidator;
        private readonly Mock<WebsiteParser> _websiteParser;

        private readonly PageCrawler _pageCrawler;

        public PageCrawlerTests()
        {
            _contentLoaderService = new Mock<IHttpClientService>();
            _urlNormalizer = new Mock<IUrlNormalizer>();
            _urlValidator = new Mock<ISimpleUrlValidator>();

            _websiteParser = new Mock<WebsiteParser>(_urlValidator.Object, _urlNormalizer.Object,
                _contentLoaderService.Object);

            var logger = new Mock<ILogger<PageCrawler>>();

            _pageCrawler = new PageCrawler(_websiteParser.Object, logger.Object);
        }

        [Fact]
        public void Crawl_WhenUrlIsNullOrEmpty_ShouldReturnEmptyList()
        {
            var resultForNull = _pageCrawler.Crawl(null);
            var resultForEmpty = _pageCrawler.Crawl(string.Empty);

            Assert.Empty(resultForNull);
            Assert.Empty(resultForEmpty);
        }

        [Fact]
        public void Crawl_WhenUrlIsValid_ShouldReturnListOfUrls()
        {
            SetupDataForWebParserUrlNormalizerUrlValidator();

            var result = _pageCrawler.Crawl("https://jwt.io/");

            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count());
            Assert.True(result.All(u => u.IsInWebsite));
            Assert.Contains(result, u => u.Url == "https://jwt.io");
            Assert.Contains(result, u => u.Url == "https://jwt.io/libraries");
            Assert.Contains(result, u => u.Url == "https://jwt.io/introduction");
        }

        private void SetupDataForWebParserUrlNormalizerUrlValidator()
        {
            _websiteParser.Setup(w => w.Parse("https://jwt.io/"))
                .Returns(new[] { "https://jwt.io/introduction", "https://jwt.io/libraries" });

            _websiteParser.Setup(w => w.Parse("https://jwt.io/introduction"))
                .Returns(new[] { "https://jwt.io", "https://jwt.io/libraries" });

            _websiteParser.Setup(w => w.Parse("https://jwt.io/libraries"))
                .Returns(new[] { "https://jwt.io", "https://jwt.io/introduction" });


            _urlNormalizer.Setup(n =>
                    n.NormalizeUrls(new[]
                        {
                            "https://jwt.io/", "https://jwt.io/introduction", "https://jwt.io/libraries"
                        },
                        "https://jwt.io/"))
                .Returns(SetupNormalizedUrls());

            _urlValidator.Setup(v => v.IsValid("https://jwt.io"))
                .Returns(true);
            _urlValidator.Setup(v => v.IsValid("https://jwt.io/introduction"))
                .Returns(true);
            _urlValidator.Setup(v => v.IsValid("https://jwt.io/libraries"))
                .Returns(true);
        }

        private IEnumerable<string> SetupNormalizedUrls()
        {
            return new List<string>()
            {
                "https://jwt.io",
                "https://jwt.io/libraries",
                "https://jwt.io/introduction"
            };
        }
    }
}