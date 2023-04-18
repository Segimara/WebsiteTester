using WebsiteTester.Domain.Models;
using WebsiteTester.Models;
using WebsiteTester.Persistenсe;

namespace WebsiteTester.Presentation.Services
{
    public class ResultsSaverService
    {
        private readonly WebsiteTesterDbContext _dbContext;

        public ResultsSaverService(WebsiteTesterDbContext context)
        {
            _dbContext = context;
        }

        public virtual async Task SaveResultsAsync(string testedUrl, IEnumerable<WebLink> testResults)
        {
            var testedLink = new Link
            {
                Url = testedUrl
            };

            await _dbContext.Links.AddAsync(testedLink);

            var webLinks = testResults.Select(r => new LinkTestResult
            {
                LinkId = testedLink.Id,
                Link = testedLink,
                Url = r.Url,
                IsInSitemap = r.IsInSitemap,
                IsInWebsite = r.IsInWebsite,
                RenderTimeMilliseconds = r.RenderTimeMilliseconds,
                CreatedOn = DateTimeOffset.UtcNow,
            });

            await _dbContext.LinkTestResults.AddRangeAsync(webLinks);
            await _dbContext.SaveChangesAsync();
        }
    }
}
