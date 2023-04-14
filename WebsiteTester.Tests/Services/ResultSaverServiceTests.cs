using Microsoft.EntityFrameworkCore;
using WebsiteTester.Domain;
using WebsiteTester.Models;
using WebsiteTester.Persistenсe;
using WebsiteTester.Services;
using WebsiteTester.Tests.Common;
using Xunit;

namespace WebsiteTester.Tests.Services
{
    public class ResultSaverServiceTests : IDisposable
    {
        private readonly WebsiteTesterDbContext _dbContext;
        private readonly ResultsSaverService _resultsSaverService;
        public ResultSaverServiceTests()
        {
            _dbContext = WebsiteTesterContextFactory.Create();
            _resultsSaverService = new ResultsSaverService(_dbContext);
        }

        [Fact]
        public async Task SaveResultsAsync_AddsNewTestedLinkToDatabase()
        {
            // Arrange
            var testedUrl = "http://example.com";
            var testResults = new List<WebLink>
        {
            new WebLink { Url = "http://example.com/page1.html", IsInSitemap = true, IsInWebsite = true, RenderTimeMilliseconds = 100 },
            new WebLink { Url = "http://example.com/page2.html", IsInSitemap = true, IsInWebsite = false, RenderTimeMilliseconds = 200 },
            new WebLink { Url = "http://example.com/page3.html", IsInSitemap = false, IsInWebsite = true, RenderTimeMilliseconds = 300 }
        };

            // Act
            await _resultsSaverService.SaveResultsAsync(testedUrl, testResults);

            // Assert
            var testedLink = await _dbContext.TestedLink.FirstOrDefaultAsync(u => u.Url == testedUrl);
            Assert.NotNull(testedLink);
            Assert.Equal(testedUrl, testedLink.Url);
        }

        [Fact]
        public async Task SaveResultsAsync_AddsLinkTestResultsToExistingTestedLink()
        {
            // Arrange
            var testedUrl = "http://example.com";
            var testResults = new List<WebLink>
            {
                new WebLink { Url = "http://example.com/page1.html", IsInSitemap = true, IsInWebsite = true, RenderTimeMilliseconds = 100 },
                new WebLink { Url = "http://example.com/page2.html", IsInSitemap = true, IsInWebsite = false, RenderTimeMilliseconds = 200 },
                new WebLink { Url = "http://example.com/page3.html", IsInSitemap = false, IsInWebsite = true, RenderTimeMilliseconds = 300 }
            };

            // Create a new TestedLink with the specified URL and add it to the database
            var testedLink = new TestedLink { Url = testedUrl };

            // Act
            await _resultsSaverService.SaveResultsAsync(testedUrl, testResults);

            // Assert
            var linkTestResults = await _dbContext.LinkTestResult.Where(r => r.TestedLinkId == testedLink.Url).ToListAsync();
            Assert.Equal(testResults.Count, linkTestResults.Count);
            foreach (var testResult in testResults)
            {
                var linkTestResult = linkTestResults.FirstOrDefault(r => r.Url == testResult.Url);
                Assert.NotNull(linkTestResult);
                Assert.Equal(testResult.IsInSitemap, linkTestResult.IsInSitemap);
                Assert.Equal(testResult.IsInWebsite, linkTestResult.IsInWebsite);
                Assert.Equal(testResult.RenderTimeMilliseconds, linkTestResult.RenderTimeMilliseconds);
            }
        }

        public void Dispose()
        {
            WebsiteTesterContextFactory.Destroy(_dbContext);
        }
    }
}
