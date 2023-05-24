using Moq;
using WebsiteTester.Crawler.Normalizers;
using WebsiteTester.Crawler.Parsers;
using WebsiteTester.Crawler.Services;
using WebsiteTester.Crawler.Validators;
using Xunit;

namespace WebsiteTester.Tests.Parsers
{
    public class WebsiteParserTests
    {
        private readonly Mock<HttpClientService> _contentLoaderService;
        private readonly Mock<SimpleUrlValidator> _urlValidator;
        private readonly Mock<IUrlNormalizer> _urlNormalizer;

        private readonly WebsiteParser _websiteParser;

        public WebsiteParserTests()
        {
            _contentLoaderService = new Mock<HttpClientService>();
            _urlValidator = new Mock<SimpleUrlValidator>();
            _urlNormalizer = new Mock<IUrlNormalizer>();

            _websiteParser =
                new WebsiteParser(_urlValidator.Object, _urlNormalizer.Object, _contentLoaderService.Object);
        }

        [Fact]
        public void Parse_WhenUrlIsNotValid_ShouldReturnEmptyList()
        {
            var invalidUrl = "this is not a valid url";

            _urlValidator.Setup(uv => uv.IsValid(invalidUrl))
                .Returns(false);

            Assert.Throws<UriFormatException>(() => _websiteParser.Parse(invalidUrl));
        }

        [Fact]
        public void Parse_WhenUrlIsValid_ShouldReturnValidUrls()
        {
            var url = "https://example.com";

            var urls = new string[]{
                "https://example.com/page1",
                "https://example.com/page2" };
            _contentLoaderService.Setup(cls => cls.GetAttributeValueOfDescendants(new Uri(url), "href", "a"))
                .Returns(urls);

            _urlValidator.Setup(uv => uv.IsValid(It.IsAny<string>())).Returns(true);

            _urlNormalizer.Setup(un => un.NormalizeUrls(It.IsAny<IEnumerable<string>>(), url))
                .Returns(new List<string>() { "https://example.com/page1", "https://example.com/page2" });

            var result = _websiteParser.Parse(url);

            Assert.Equal(2, result.Count());
            Assert.Contains("https://example.com/page1", result);
            Assert.Contains("https://example.com/page2", result);
        }

        [Fact]
        public void Parse_WhenUrlIsNotHtml_ShouldThrowException()
        {
            var url = "https://example.com/image.png";

            _contentLoaderService.Setup(cls => cls.GetAttributeValueOfDescendants(new Uri(url), "href", "a"))
                .Throws<Exception>();

            Assert.Throws<Exception>(() => _websiteParser.Parse(url));
        }
    }
}