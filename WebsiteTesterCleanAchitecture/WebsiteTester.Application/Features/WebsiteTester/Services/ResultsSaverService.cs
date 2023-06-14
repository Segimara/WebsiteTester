using WebsiteTester.Application.Common.Interfaces;
using WebsiteTester.Application.Features.WebsiteTester.Crawlers;
using WebsiteTester.Application.Features.WebsiteTester.Validators.Interfaces;
using WebsiteTester.Application.Models;
using WebsiteTester.Domain.Models;

namespace WebsiteTester.Application.Features.WebsiteTester.Services
{
    public class ResultsSaverService : IResultsSaverService
    {
        private readonly IWebsiteTesterDbContext _dbContext;
        private readonly IWebsiteCrawler _websiteCrawler;
        private readonly IComplexUrlValidator _urlValidator;

        public ResultsSaverService(IWebsiteTesterDbContext context, IWebsiteCrawler websiteCrawler, IComplexUrlValidator urlValidator)
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
                return ex;
            }

            var links = await _websiteCrawler.GetUrlsAsync(url);

            await SaveResultsAsync(url, links);
            return true;
        }

        public async Task SaveResultsAsync(string testedUrl, IEnumerable<WebLink> testResults)
        {
            var testedLink = new Link
            {
                Url = testedUrl,
                CreatedOn = DateTimeOffset.UtcNow,
            };

            _dbContext.Add(testedLink);

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

            _dbContext.AddRange(webLinks);
            await _dbContext.SaveChangesAsync();
        }
    }
}
