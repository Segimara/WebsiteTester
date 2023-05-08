using Microsoft.AspNetCore.Mvc;
using WebsiteTester.Web.Logic.Models;
using WebsiteTester.Web.Logic.Services;
using WebsiteTester.WebApi.Extensions;

namespace WebsiteTester.WebApi.Controllers
{
    public class WebsiteTesterController : BaseController
    {
        private readonly ResultsSaverService _resultsSaverService;
        private readonly ResultsReceiverService _resultsReceiverService;
        public WebsiteTesterController(ResultsReceiverService resultsReceiverService, ResultsSaverService resultsSaverService)
        {
            _resultsSaverService = resultsSaverService;
            _resultsReceiverService = resultsReceiverService;
        }

        [HttpGet("/tests")]
        public async Task<ActionResult<IEnumerable<Link>>> TestResults()
        {
            var result = await _resultsReceiverService.GetResultsAsync();
            return Ok(result);
        }
        [HttpGet("/tests/{id}")]
        public async Task<ActionResult<Link>> TestDetails(Guid id)
        {
            var result = await _resultsReceiverService.GetTestDetailAsync(id.ToString());
            return result.ToApiResponseResult();
        }

        [HttpPost("/test/{url}")]
        public async Task<ActionResult<bool>> TestWebsite(string url)
        {
            var result = await _resultsSaverService.GetAndSaveResultsAsync(System.Web.HttpUtility.UrlDecode(url));
            return result.ToApiResponseResult();
        }

    }
}
