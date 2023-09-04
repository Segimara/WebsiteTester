using HtmlAgilityPack;
using Moq;
using WebsiteTester.Crawler.Services;
using WebsiteTester.Crawler.Utility;
using WebsiteTester.Domain.InternalModels;
using WebsiteTester.Domain.Models;
using Xunit;

namespace WebsiteTester.Tests.Services
{
    public class TimeMeterServiceTests
    {

        private readonly Mock<HttpClientService> _httpClientService;
        private readonly TimeMeterUtility _renderTimeMeter;

        public TimeMeterServiceTests()
        {
            var htmlweb = new Mock<HtmlWeb>();
            _httpClientService = new Mock<HttpClientService>(htmlweb.Object);
            _renderTimeMeter = new TimeMeterUtility(_httpClientService.Object);
        }

        [Fact]
        public async void TestRenderTimeAsync_WhenContainsWebLinks_ShouldReturnListWithRenderTime()
        {
            var urls = new[]
            {
                new WebLink
                {
                    Url = "https://www.google.com"
                }
            };

            _httpClientService.Setup(x => x.GetRenderTime(It.IsAny<Uri>()))
                .Returns(() =>
            {
                return 5;
            });

            var result = await _renderTimeMeter.TestRenderTimeAsync(urls);

            Assert.True(result.All(u => u.RenderTimeMilliseconds > 0));
        }

    }
}
