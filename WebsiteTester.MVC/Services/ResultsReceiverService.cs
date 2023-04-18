using WebsiteTester.MVC.Models;
using WebsiteTester.Persistence;

namespace WebsiteTester.MVC.Services
{
    public class ResultsReceiverService
    {
        private readonly WebsiteTesterDbContext _dbContext;
        public ResultsReceiverService(WebsiteTesterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Link>> GetResultsAsync()
        {
            var links = _dbContext.Links.Select(l =>
            new Link
            {
                Id = l.Id,
                Url = l.Url,
                CreatedOn = l.CreatedOn
            })
               .AsEnumerable();

            return links;
        }

        public async Task<TestDetail> GetTestDetailAsync(string id)
        {
            var link = _dbContext.Links
                .Where(l => l.Id == Guid.Parse(id))
                .Select(l =>
                new Link
                {
                    Id = l.Id,
                    Url = l.Url,
                    CreatedOn = l.CreatedOn
                })
                .FirstOrDefault();

            if (link == null)
            {
                return new TestDetail
                {
                    TestedUrl = "There is no url to this id",
                    TestResults = Enumerable.Empty<TestResult>()
                };
            }

            var testResults = _dbContext.LinkTestResults
                .Where(l => l.LinkId == Guid.Parse(id))
                .Select(l =>
                new TestResult
                {
                    Url = l.Url,
                    IsInSitemap = l.IsInSitemap,
                    IsInWebsite = l.IsInWebsite,
                    RenderTimeMilliseconds = l.RenderTimeMilliseconds,
                    CreatedOn = l.CreatedOn
                })
                .AsEnumerable();

            var testDetail = new TestDetail
            {
                TestedUrl = link.Url,
                TestResults = testResults
            };

            return testDetail;
        }
    }
}
