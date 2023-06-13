using LanguageExt.Common;
using WebsiteTester.Crawlers;
using WebsiteTester.Models;
using WebsiteTester.Persistence;
using WebsiteTester.Web.Logic.Validators;

namespace WebsiteTester.Web.Logic.Services
{
    public class ResultsSaverService
    {
        private readonly WebsiteTesterDbContext _dbContext;
        private readonly WebsiteCrawler _websiteCrawler;
        private readonly UrlValidator _urlValidator;

        public ResultsSaverService(WebsiteTesterDbContext context, WebsiteCrawler websiteCrawler, UrlValidator urlValidator)
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

            await _dbContext.Links.AddAsync(testedLink);

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

            await _dbContext.LinkTestResults.AddRangeAsync(webLinks);
            await _dbContext.SaveChangesAsync();
        }
    }
}
