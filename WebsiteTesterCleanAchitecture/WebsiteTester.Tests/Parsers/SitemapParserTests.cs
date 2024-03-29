﻿using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Moq;
using WebsiteTester.Crawler.Normalizers;
using WebsiteTester.Crawler.Parsers;
using WebsiteTester.Crawler.Services;
using WebsiteTester.Crawler.Validators.Interfaces;
using Xunit;

namespace WebsiteTester.Tests.Parsers;

public class SitemapParserTests
{
    private readonly Mock<HttpClientService> _httpClientService;
    private readonly SitemapParser _sitemapParser;
    private readonly Mock<IUrlNormalizer> _urlNormalizer;
    private readonly Mock<ISimpleUrlValidator> _urlValidator;

    public SitemapParserTests()
    {

        var htmlweb = new Mock<HtmlWeb>();
        _httpClientService = new Mock<HttpClientService>(htmlweb.Object);
        _urlNormalizer = new Mock<IUrlNormalizer>();
        _urlValidator = new Mock<ISimpleUrlValidator>();
        var logger = new Mock<ILogger<SitemapParser>>();

        _sitemapParser = new SitemapParser(_urlValidator.Object, _urlNormalizer.Object, _httpClientService.Object, logger.Object);
    }

    [Fact]
    public async Task Parse_WebsiteWithoutSitemap_ShouldReturnEmpryList()
    {
        var uri = new Uri("https://jwt.io/");

        var result = await _sitemapParser.ParseAsync("https://www.google.com/");

        _httpClientService.Setup(h => h.GetContent(uri)).Returns(
            new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.BadGateway
            });

        Assert.Empty(result);
    }

    [Fact]
    public async Task Parse_WebsiteWithEmptySitemap_ShouldReturnEmptyList()
    {
        var uri = new Uri("https://jwt.io/");

        _httpClientService.Setup(h => h.GetContent(uri)).Returns(
            new HttpResponseMessage()
            {
                Content = new StringContent(
                    @"<?xml version=""1.0"" encoding=""UTF-8""?>
                    <urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
                    </urlset>")
            });

        var result = await _sitemapParser.ParseAsync("https://www.google.com/");

        Assert.Empty(result);
    }


    [Fact]
    public async void Parse_WebsiteWithSitemap_ShouldReturnListOfUrls()
    {
        var uri = new Uri("https://jwt.io/");

        _httpClientService.Setup(h => h.GetContent(It.IsAny<Uri>()))
            .Returns(SetupHttpResponseMessage());

        _urlNormalizer.Setup(n => n.NormalizeUrls(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Returns(SetupNormalizedUrls());

        _urlValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);

        var result = await _sitemapParser.ParseAsync("https://jwt.io/");

        _urlNormalizer.Verify(n =>
            n.NormalizeUrls(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()), Times.Once);

        Assert.Equal(3, result.Count());
        Assert.True(result.All(u => u.IsInSitemap));
        Assert.Contains(result, u => u.Url == "https://jwt.io");
        Assert.Contains(result, u => u.Url == "https://jwt.io/libraries");
        Assert.Contains(result, u => u.Url == "https://jwt.io/introduction");
    }

    private HttpResponseMessage SetupHttpResponseMessage()
    {
        return new HttpResponseMessage()
        {
            StatusCode = System.Net.HttpStatusCode.OK,

            Content = new StringContent(
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                    <urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
                        <url>
                            <loc>https://jwt.io/</loc>
                        </url>
                        <url>
                            <loc>https://jwt.io/libraries/</loc>
                        </url>
                        <url>
                            <loc>https://jwt.io/introduction/</loc>
                        </url>
                    </urlset>")
        };
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