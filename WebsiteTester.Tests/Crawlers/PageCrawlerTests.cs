using HtmlAgilityPack;
using Moq;
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
        private readonly Mock<ContentLoaderService> _contentLoaderService;
        private readonly Mock<UrlNormalizer> _urlNormalizer;
        private readonly Mock<UrlValidator> _urlValidator;
        private readonly Mock<WebsiteParser> _websiteParser;

        private readonly PageCrawler _pageCrawler;

        public PageCrawlerTests()
        {
            var htmlWeb = new HtmlWeb();

            _contentLoaderService = new Mock<ContentLoaderService>(htmlWeb);
            _urlNormalizer = new Mock<UrlNormalizer>();
            _urlValidator = new Mock<UrlValidator>();

            _websiteParser = new Mock<WebsiteParser>(_urlValidator.Object, _urlNormalizer.Object,
                _contentLoaderService.Object);

            _pageCrawler = new PageCrawler(_websiteParser.Object);
        }

        [Fact]
        public void Crawl_WhenUrlIsNullOrEmpty_ShouldReturnEmptyList()
        {
            var result1 = _pageCrawler.Crawl(null);
            var result2 = _pageCrawler.Crawl(string.Empty);

            Assert.Empty(result1);
            Assert.Empty(result2);
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
                .Returns(new[]
                    { "https://jwt.io/introduction", "https://jwt.io/libraries" });

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

            _urlValidator.Setup(v => v.IsValid("https://jwt.io")).Returns(true);
            _urlValidator.Setup(v => v.IsValid("https://jwt.io/introduction")).Returns(true);
            _urlValidator.Setup(v => v.IsValid("https://jwt.io/libraries")).Returns(true);
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