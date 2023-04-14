using WebsiteTester.Common.Interfaces;
using WebsiteTester.Domain;
using WebsiteTester.Models;

namespace WebsiteTester.Services
{
    public class ResultsSaverService
    {
        private readonly IWebsiteTesterDbContext _dbContext;
        public ResultsSaverService(IWebsiteTesterDbContext context)
        {
            _dbContext = context;
        }
        public async Task SaveResultsAsync(string testedUrl, IEnumerable<WebLink> testResults)
        {
            var testedLink = _dbContext.TestedLink.FirstOrDefault(u => u.Url == testedUrl);
            if (testedLink == null)
            {
                testedLink = new TestedLink
                {
                    Url = testedUrl
                };
            }

            var webLinks = testResults.Select(r => new LinkTestResult
            {
                TestedLink = testedLink,
                Id = Guid.NewGuid(),
                Url = r.Url,
                IsInSitemap = r.IsInSitemap,
                IsInWebsite = r.IsInWebsite,
                RenderTimeMilliseconds = r.RenderTimeMilliseconds,
                CreatedOn = DateTimeOffset.Now,
            });

            await _dbContext.LinkTestResult.AddRangeAsync(webLinks);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }
    }
}
