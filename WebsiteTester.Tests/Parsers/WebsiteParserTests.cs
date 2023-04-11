using HtmlAgilityPack;
using Moq;
using WebsiteTester.Normalizers;
using WebsiteTester.Parsers;
using WebsiteTester.Services;
using WebsiteTester.Validators;
using Xunit;

namespace WebsiteTester.Tests.Parsers
{
    public class WebsiteParserTests
    {
        Mock<ContentLoaderService> _contentLoaderService;
        Mock<UrlValidator> _urlValidator;
        Mock<UrlNormalizer> _urlNormalizer;

        WebsiteParser _websiteParser;

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
        public void Parse_WhenUrlIsNull_ShouldReturnEmptyList()
        {
            Assert.Throws<ArgumentNullException>(() => _websiteParser.Parse(null));
        }
    }
}