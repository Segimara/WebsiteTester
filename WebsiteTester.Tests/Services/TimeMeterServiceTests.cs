using Moq;
using WebsiteTester.Models;
using WebsiteTester.Services;
using Xunit;

namespace WebsiteTester.Tests.Services
{
    public class TimeMeterServiceTests
    {
        private readonly Mock<HttpClientService> _httpClientService;
        private readonly TimeMeterService _renderTimeMeter;

        public TimeMeterServiceTests()
        {
            HttpClient _webClient = new HttpClient();
            _httpClientService = new Mock<HttpClientService>(_webClient);
            _renderTimeMeter = new TimeMeterService(_httpClientService.Object);
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

            _httpClientService.Setup(x => x.GetAsync(It.IsAny<Uri>())).ReturnsAsync(() =>
            {
                Task.Delay(TimeSpan.FromMilliseconds(1)).Wait();
                return new HttpResponseMessage();
            });

            var result = await _renderTimeMeter.TestRenderTimeAsync(urls);

            Assert.True(result.All(u => u.RenderTimeMilliseconds > 0));
        }

    }
}
