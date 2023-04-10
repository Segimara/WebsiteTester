using Moq;
using WebsiteTester.Normalizers;
using WebsiteTester.Parsers;
using WebsiteTester.Services;
using WebsiteTester.Validators;
using Xunit;

namespace WebsiteTester.Tests.Parsers;

public class SitemapParserTests
{
    private readonly Mock<HttpClientService> _httpClientService;
    private readonly SitemapParser _sitemapParser;
    private readonly Mock<UrlNormalizer> _urlNormalizer;
    private readonly Mock<UrlValidator> _urlValidator;

    public SitemapParserTests()
    {
        var httpClient = new HttpClient();

        _httpClientService = new Mock<HttpClientService>(httpClient);
        _urlNormalizer = new Mock<UrlNormalizer>();
        _urlValidator = new Mock<UrlValidator>();

        _sitemapParser = new SitemapParser(_urlValidator.Object, _urlNormalizer.Object, _httpClientService.Object);
    }

    [Fact]
    public void Parse_WebsiteWithoutSitemap_ShouldReturnEmpryList()
    {
        var result = _sitemapParser.Parse("https://learn.microsoft.com/");

        Assert.Empty(result);
    }

    [Fact]
    public void Parse_WebsiteWithSitemapButWithoutUrls_ShouldReturnEmptyList()
    {
        var result = _sitemapParser.Parse("https://www.google.com/");

        Assert.NotEmpty(result);
    }

    [Fact]
    public void Parse_WebsiteWithSitemapAndUrls_ShouldReturnListOfUrls()
    {
        var result = _sitemapParser.Parse("https://jwt.io/");

        _urlNormalizer.Setup(n =>
                n.NormalizeUrls(new [] { "https://jwt.io/", "https://jwt.io/libraries/", "https://jwt.io/introduction/" },
                    "https://jwt.io/"))
            .Returns(new [] { "https://jwt.io", "https://jwt.io/libraries", "https://jwt.io/introduction" });
        
        _urlValidator.Setup(v => v.IsValid("https://jwt.io")).Returns(true);
        _urlValidator.Setup(v => v.IsValid("https://jwt.io/libraries")).Returns(true);
        _urlValidator.Setup(v => v.IsValid("https://jwt.io/introduction")).Returns(true);


        _urlNormalizer.Verify(n =>
                           n.NormalizeUrls(new[] { "https://jwt.io/", "https://jwt.io/libraries/", "https://jwt.io/introduction/" },
                                                  "https://jwt.io/"), Times.Once);
        _urlValidator.Verify(v => v.IsValid("https://jwt.io"), Times.Once);
        _urlValidator.Verify(v => v.IsValid("https://jwt.io/libraries"), Times.Once);
        _urlValidator.Verify(v => v.IsValid("https://jwt.io/introduction"), Times.Once);

        Assert.Equal(3, result.Count());
    }
}