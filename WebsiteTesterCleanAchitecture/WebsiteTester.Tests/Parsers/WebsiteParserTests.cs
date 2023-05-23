using Moq;
using Xunit;

namespace WebsiteTester.Tests.Parsers
{
    public class WebsiteParserTests
    {
        private readonly Mock<ContentLoaderService> _contentLoaderService;
        private readonly Mock<UrlValidator> _urlValidator;
        private readonly Mock<UrlNormalizer> _urlNormalizer;

        private readonly WebsiteParser _websiteParser;

        public WebsiteParserTests()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            _contentLoaderService = new Mock<ContentLoaderService>(htmlWeb);
            _urlValidator = new Mock<UrlValidator>();
            _urlNormalizer = new Mock<UrlNormalizer>();

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

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(@"!<DOCTYPE html><html><body>
                                    <a href='https://example.com/page1'>Page 1</a>
                                    <a href='https://example.com/page2'>Page 2</a></body></html>");
            _contentLoaderService.Setup(cls => cls.Load(new Uri(url)))
                .Returns(htmlDoc);

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
            var html = "<html><body><img src='https://example.com/image.png'></body></html>";

            _contentLoaderService.Setup(cls => cls.Load(new Uri(url)))
                .Returns(new HtmlDocument() { Text = html });

            Assert.Throws<Exception>(() => _websiteParser.Parse(url));
        }
    }
}