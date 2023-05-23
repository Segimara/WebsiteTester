using WebsiteTester.Application.Common.Interfaces;
using WebsiteTester.Application.Models;
using WebsiteTester.Application.WebsiteTester.Crawlers;
using WebsiteTester.Application.WebsiteTester.Models;
using WebsiteTester.Application.WebsiteTester.Validators.Interfaces;

namespace WebsiteTester.Application.WebsiteTester.Services
{
    public class ResultsSaverService : IResultsSaverService
    {
        private readonly IWebsiteTesterDbContext _dbContext;
        private readonly WebsiteCrawler _websiteCrawler;
        private readonly IComplexUrlValidator _urlValidator;

        public ResultsSaverService(IWebsiteTesterDbContext context, WebsiteCrawler websiteCrawler, IComplexUrlValidator urlValidator)
        {
            _dbContext = context;
            _websiteCrawler = websiteCrawler;
            _urlValidator = urlValidator;
        }

        public async Task<Result<bool>> GetAndSaveResultsAsync(string url)
        {
            try
            {
                _urlValidator.IsValid(url);
            }
            catch (Exception ex)
            {
                return new Result<bool>(ex);
            }

            var links = await _websiteCrawler.GetUrlsAsync(url);

            await SaveResultsAsync(url, links);
            return true;
        }

        public async Task SaveResultsAsync(string testedUrl, IEnumerable<WebLink> testResults)
        {
            var testedLink = new Domain.Models.Link
            {
                Url = testedUrl,
                CreatedOn = DateTimeOffset.UtcNow,
            };

            _dbContext.Links.Append(testedLink);

            var webLinks = testResults.Select(r => new Domain.Models.LinkTestResult
            {
                LinkId = testedLink.Id,
                Link = testedLink,
                Url = r.Url,
                IsInSitemap = r.IsInSitemap,
                IsInWebsite = r.IsInWebsite,
                RenderTimeMilliseconds = r.RenderTimeMilliseconds,
                CreatedOn = DateTimeOffset.UtcNow,
            });

            _dbContext.LinkTestResults.Concat(webLinks);
            await _dbContext.SaveChangesAsync();
        }
    }
}
