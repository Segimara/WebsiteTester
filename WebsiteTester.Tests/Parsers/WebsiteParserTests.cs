﻿using HtmlAgilityPack;
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
        public void Parse_WhenUrlIsNotValid_ShouldReturnEmptyList()
        {
            // Arrange
            var invalidUrl = "this is not a valid url";
            _urlValidator.Setup(uv => uv.IsValid(invalidUrl)).Returns(false);


            // Assert
            Assert.Throws<UriFormatException>(() => _websiteParser.Parse(invalidUrl));
        }

        [Fact]
        public void Parse_WhenUrlIsValid_ShouldReturnValidUrls()
        {
            // Arrange
            var url = "https://example.com";
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(@"!<DOCTYPE html><html><body>
                                    <a href='https://example.com/page1'>Page 1</a>
                                    <a href='https://example.com/page2'>Page 2</a></body></html>");
            _contentLoaderService.Setup(cls => cls.Load(new Uri(url))).Returns(htmlDoc);
            _urlValidator.Setup(uv => uv.IsValid(It.IsAny<string>())).Returns(true);
            _urlNormalizer.Setup(un => un.NormalizeUrls(It.IsAny<IEnumerable<string>>(), url)).Returns(new List<string>() { "https://example.com/page1", "https://example.com/page2" });

            // Act
            var result = _websiteParser.Parse(url);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains("https://example.com/page1", result);
            Assert.Contains("https://example.com/page2", result);
        }

        [Fact]
        public void Parse_WhenUrlIsNotHtml_ShouldThrowException()
        {
            // Arrange
            var url = "https://example.com/image.png";
            var html = "<html><body><img src='https://example.com/image.png'></body></html>";
            _contentLoaderService.Setup(cls => cls.Load(new Uri(url))).Returns(new HtmlDocument() { Text = html });

            // Act & Assert
            Assert.Throws<Exception>(() => _websiteParser.Parse(url));
        }
    }
}