using WebsiteTester.Application.Common.Interfaces;
using WebsiteTester.Application.Features.WebsiteTester.DtoModels;
using WebsiteTester.Application.Models;

namespace WebsiteTester.Application.Features.WebsiteTester.Services
{
    public class ResultsReceiverService : IResultsReceiverService
    {
        private readonly IWebsiteTesterDbContext _dbContext;
        public ResultsReceiverService(IWebsiteTesterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Link>> GetResultsAsync(Guid userId)
        {
            return _dbContext.Links
                .Where(l => l.UserID.Equals(userId))
                .Select(l => new Link
                {
                    Id = l.Id,
                    Url = l.Url,
                    CreatedOn = l.CreatedOn,
                })
               .ToList();
        }

        public async Task<Result<Link>> GetTestDetailAsync(Guid userId, string id)
        {
            var link = _dbContext.Links
                .Where(l => l.UserID.Equals (userId))
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
                return new Exception("Link by that id not found");
            }

            var testResults = _dbContext.LinkTestResults
                .Where(l => l.LinkId == Guid.Parse(id))
                .Select(l => new TestResult
                {
                    Url = l.Url,
                    IsInSitemap = l.IsInSitemap,
                    IsInWebsite = l.IsInWebsite,
                    RenderTimeMilliseconds = l.RenderTimeMilliseconds,
                    CreatedOn = l.CreatedOn
                })
                .ToList();

            return new Link
            {
                Url = link.Url,
                TestResults = testResults
            };
        }
    }
}
