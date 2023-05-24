using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Moq;
using WebsiteTester.Application.Common.Interfaces;
using WebsiteTester.Crawler.Crawlers;
using WebsiteTester.Crawler.Interfaces;
using WebsiteTester.Crawler.Models;
using WebsiteTester.Crawler.Normalizers;
using WebsiteTester.Crawler.Parsers;
using WebsiteTester.Crawler.Utility;
using WebsiteTester.Crawler.Validators.Interfaces;
using Xunit;

namespace WebsiteTester.Tests.Crawlers
{
    public class WebsiteCrawlerTests
    {
        private readonly Mock<TimeMeterUtility> _renderTimeMeter;
        private readonly Mock<SitemapParser> _siteMapParser;
        private readonly Mock<PageCrawler> _pageCrawler;

        private readonly WebsiteCrawler _websiteCrawler;

        public WebsiteCrawlerTests()
        {
            var htmlWeb = new HtmlWeb();
            var httpClient = new HttpClient();
            var contentLoaderService = new Mock<IHttpClientService>(htmlWeb);
            var urlValidator = new Mock<ISimpleUrlValidator>();
            var urlNormalizer = new Mock<IUrlNormalizer>();
            var parser = new WebsiteParser(urlValidator.Object, urlNormalizer.Object, contentLoaderService.Object);
            var logger = new Mock<ILogger>();

            _siteMapParser = new Mock<SitemapParser>(urlValidator.Object, urlNormalizer.Object, contentLoaderService.Object, logger.Object);
            _renderTimeMeter = new Mock<TimeMeterUtility>(contentLoaderService.Object);
            _pageCrawler = new Mock<PageCrawler>(parser, logger.Object);

            var dbContext = new Mock<IWebsiteTesterDbContext>();
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