using Moq;
using WebsiteTester.Crawlers;
using WebsiteTester.Models;
using Xunit;

namespace WebsiteTester.Presentation.Tests
{
    public class WebsiteTesterApplicationTests
    {
        private readonly Mock<WebsiteCrawler> _crawlerMock;
        private readonly Mock<ConsoleManager> _consoleMock;
        private readonly WebsiteTesterApplication _websiteTester;

        public WebsiteTesterApplicationTests()
        {
            _crawlerMock = new Mock<WebsiteCrawler>();
            _consoleMock = new Mock<ConsoleManager>();

            _websiteTester = new WebsiteTesterApplication(_crawlerMock.Object, _consoleMock.Object);
        }

        [Fact]
        public async Task Run_ValidUrl_CrawlsWebsiteAndOutputsResults()
        {
            // Arrange
            var url = "https://example.com";

            var webLinks = new List<WebLink>()
        {
            new WebLink() { Url = "https://example.com/page1", IsInWebsite = true, IsInSitemap = false, RenderTimeMilliseconds = 500 },
            new WebLink() { Url = "https://example.com/page2", IsInWebsite = true, IsInSitemap = false, RenderTimeMilliseconds = 100 },
            new WebLink() { Url = "https://example.com/page3", IsInWebsite = true, IsInSitemap = true, RenderTimeMilliseconds = 200 },
            new WebLink() { Url = "https://example.com/page4", IsInWebsite = false, IsInSitemap = true, RenderTimeMilliseconds = 300 }
        };

            _crawlerMock.Setup(c => c.GetUrlsAsync(url)).ReturnsAsync(webLinks);

            // Act
            await _websiteTester.Run();

            // Assert
            _consoleMock.Verify(c => c.WriteLine("Enter the website URL: "), Times.Once);
            _consoleMock.Verify(c => c.ReadLine(), Times.Once);

            _consoleMock.Verify(c => c.WriteLine("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site"), Times.Once);
            _consoleMock.Verify(c => c.WriteLine("1) https://example.com/page4"), Times.Once);

            _consoleMock.Verify(c => c.WriteLine("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml"), Times.Once);
            _consoleMock.Verify(c => c.WriteLine("1) https://example.com/page1"), Times.Once);

            _consoleMock.Verify(c => c.WriteLine("Timing"), Times.Once);
            _consoleMock.Verify(c => c.WriteLine("1) https://example.com/page2 \t 100"), Times.Once);
            _consoleMock.Verify(c => c.WriteLine("2) https://example.com/page3 \t 200"), Times.Once);
            _consoleMock.Verify(c => c.WriteLine("3) https://example.com/page1 \t 500"), Times.Once);

            _consoleMock.Verify(c => c.WriteLine("Urls(html documents) found after crawling a website: 3"), Times.Once);
            _consoleMock.Verify(c => c.WriteLine("Urls found in sitemap: 2"), Times.Once);
        }


    }
}