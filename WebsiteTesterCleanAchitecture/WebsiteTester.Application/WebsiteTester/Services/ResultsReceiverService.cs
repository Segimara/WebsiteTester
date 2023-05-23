using WebsiteTester.Application.Common.Interfaces;
using WebsiteTester.Application.Models;
using WebsiteTester.Application.WebsiteTester.ViewModels;

namespace WebsiteTester.Web.Logic.Services
{
    public class ResultsReceiverService
    {
        private readonly IWebsiteTesterDbContext _dbContext;
        public ResultsReceiverService(IWebsiteTesterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Link>> GetResultsAsync()
        {
            return _dbContext.Links.Select(l => new Link
            {
                Id = l.Id,
                Url = l.Url,
                CreatedOn = l.CreatedOn,
            })
               .ToList();
        }

        public async Task<Result<Link>> GetTestDetailAsync(string id)
        {
            var link = await _dbContext.Links
                .Where(l => l.Id == Guid.Parse(id))
                .Select(l => new Link
                {
                    Id = l.Id,
                    Url = l.Url,
                    CreatedOn = l.CreatedOn
                })
                .FirstOrDefault();

            if (link == null)
            {
                return new Result<Link>(new Exception("Link by that id not found"));
            }

            var testResults = await _dbContext.LinkTestResults
                .Where(l => l.LinkId == Guid.Parse(id))
                .Select(l => new TestResult
                {
                    Url = l.Url,
                    IsInSitemap = l.IsInSitemap,
                    IsInWebsite = l.IsInWebsite,
                    RenderTimeMilliseconds = l.RenderTimeMilliseconds,
                    CreatedOn = l.CreatedOn
                })
                .ToListAsync();

            return new Link
            {
                Url = link.Url,
                TestResults = testResults
            };
        }
    }
}
