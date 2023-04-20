using HtmlAgilityPack;
using Moq;
using WebsiteTester.Crawlers;
using WebsiteTester.Models;
using WebsiteTester.Normalizers;
using WebsiteTester.Parsers;
using WebsiteTester.Persistence;
using WebsiteTester.Services;
using WebsiteTester.Validators;
using Xunit;

namespace WebsiteTester.Tests.Crawlers
{
    public class WebsiteCrawlerTests
    {
        private readonly Mock<TimeMeterService> _renderTimeMeter;
        private readonly Mock<SitemapParser> _siteMapParser;
        private readonly Mock<PageCrawler> _pageCrawler;

        private readonly WebsiteCrawler _websiteCrawler;

        public WebsiteCrawlerTests()
        {
            var htmlWeb = new HtmlWeb();
            var httpClient = new HttpClient();
            var contentLoaderService = new ContentLoaderService(htmlWeb);
            var httpClientService = new HttpClientService(httpClient);
            var urlValidator = new UrlValidator();
            var urlNormalizer = new UrlNormalizer();
            var parser = new WebsiteParser(urlValidator, urlNormalizer, contentLoaderService);

            _siteMapParser = new Mock<SitemapParser>(urlValidator, urlNormalizer, httpClientService);
            _renderTimeMeter = new Mock<TimeMeterService>(httpClientService);
            _pageCrawler = new Mock<PageCrawler>(parser);

            var dbContext = new Mock<WebsiteTesterDbContext>();
            _websiteCrawler = new WebsiteCrawler(_siteMapParser.Object, _pageCrawler.Object, _renderTimeMeter.Object);
        }

        [Fact]
        public async Task Crawl_WhenUrlIsNullOrEmpty_ShouldReturnEmptyList()
        {
            var resultForNull = await _websiteCrawler.GetUrlsAsync(null);
            var resultForEmpty = await _websiteCrawler.GetUrlsAsync(string.Empty);

            Assert.Empty(resultForNull);
            Assert.Empty(resultForEmpty);
        }

        [Fact]
        public async void Crawl_WhenPageContainsSomeUrls_ShouldReturnUniqueWebLinksWithRenderTime()
        {
            string url = "https://jwt.io/";

            _siteMapParser.Setup(s => s.ParseAsync(url))
                .ReturnsAsync(GetWebLinksFromSitemap());
            _pageCrawler.Setup(p => p.Crawl(url))
                .Returns(GetWebLinksFromWebSite());

            _renderTimeMeter.Setup(r =>
                    r.TestRenderTimeAsync(It.IsAny<IEnumerable<WebLink>>()))
                .ReturnsAsync(GetUniqueWebLinks());

            var result = await _websiteCrawler.GetUrlsAsync(url);

            _pageCrawler.Verify(p =>
                p.Crawl("https://jwt.io/"), Times.Once);

            _siteMapParser.Verify(s =>
                s.ParseAsync(It.IsAny<string>()), Times.Once);

            _renderTimeMeter.Verify(r =>
                r.TestRenderTimeAsync(It.IsAny<IEnumerable<WebLink>>()), Times.Once);

            Assert.Equal(3, result.Count());
        }

        private IEnumerable<WebLink> GetWebLinksFromSitemap()
        {
            return new[]
                {
                    new WebLink
                    {
                        Url = "https://jwt.io",
                        IsInSitemap = true,
                        IsInWebsite = false
                    },
                    new WebLink
                    {
                        Url = "https://jwt.io/#debugger-io",
                        IsInSitemap = true,
                        IsInWebsite = false
                    },
                };
        }

        private IEnumerable<WebLink> GetWebLinksFromWebSite()
        {
            return new[]
                {
                    new WebLink
                    {
                        Url = "https://jwt.io",
                        IsInSitemap = false,
                        IsInWebsite = true
                    },
                    new WebLink
                    {
                        Url = "https://jwt.io/libraries",
                        IsInSitemap = false,
                        IsInWebsite = true
                    }
                };
        }

        private IEnumerable<WebLink> GetUniqueWebLinks()
        {
            return new[]
                {
                    new WebLink
                    {
                        Url = "https://jwt.io",
                        IsInSitemap = true,
                        IsInWebsite = true,
                        RenderTimeMilliseconds = 10
                    },
                    new WebLink
                    {
                        Url = "https://jwt.io/#debugger-io",
                        IsInSitemap = true,
                        IsInWebsite = false,
                        RenderTimeMilliseconds = 11
                    },
                    new WebLink
                    {
                        Url = "https://jwt.io/libraries",
                        IsInSitemap = false,
                        IsInWebsite = true,
                        RenderTimeMilliseconds = 12
                    }
                };
        }
    }
}