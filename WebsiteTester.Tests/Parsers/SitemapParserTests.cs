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
    public async Task Parse_WebsiteWithoutSitemap_ShouldReturnEmpryList()
    {
        var result = await _sitemapParser.ParseAsync("https://www.google.com/");
        var uri = new Uri("https://jwt.io/");

        _httpClientService.Setup(h => h.GetAsync(uri)).ReturnsAsync(
            new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.BadGateway
            });

        Assert.Empty(result);
    }

    [Fact]
    public async Task Parse_WebsiteWithSitemapButWithoutUrls_ShouldReturnEmptyList()
    {
        var result = await _sitemapParser.ParseAsync("https://www.google.com/");
        var uri = new Uri("https://jwt.io/");

        _httpClientService.Setup(h => h.GetAsync(uri)).ReturnsAsync(
            new HttpResponseMessage()
            {
                Content = new StringContent(
                    @"<?xml version=""1.0"" encoding=""UTF-8""?>
                    <urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
                    </urlset>")
            });

        Assert.Empty(result);
    }


    [Fact]
    public async void Parse_WebsiteWithSitemapAndUrls_ShouldReturnListOfUrls()
    {
        var uri = new Uri("https://jwt.io/");

        _httpClientService.Setup(h => h.GetAsync(It.IsAny<Uri>())).ReturnsAsync(SetupHttpResponseMessage());

        _urlNormalizer.Setup(n => n.NormalizeUrls(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .Returns(new[] { "https://jwt.io", "https://jwt.io/libraries", "https://jwt.io/introduction" });

        _urlValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);

        var result = await _sitemapParser.ParseAsync("https://jwt.io/");

        _urlNormalizer.Verify(n =>
            n.NormalizeUrls(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()), Times.Once);

        Assert.Equal(3, result.Count());
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
}