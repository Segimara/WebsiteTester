using Microsoft.AspNetCore.Mvc;
using WebsiteTester.Web.Logic.Models;
using WebsiteTester.Web.Logic.Services;
using WebsiteTester.WebApi.Extensions;
using WebsiteTester.WebApi.Models;

namespace WebsiteTester.WebApi.Controllers
{
    [Route("/api/[controller]")]
    public class TestsController : BaseController
    {
        private readonly ResultsSaverService _resultsSaverService;
        private readonly ResultsReceiverService _resultsReceiverService;
        public TestsController(ResultsReceiverService resultsReceiverService, ResultsSaverService resultsSaverService)
        {
            _resultsSaverService = resultsSaverService;
            _resultsReceiverService = resultsReceiverService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Link>>> TestResults()
        {
            var result = await _resultsReceiverService.GetResultsAsync();
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Link>> TestDetails(Guid id)
        {
            var result = await _resultsReceiverService.GetTestDetailAsync(id.ToString());
            return result.ToApiResponseResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Url for performance testing </param>
        /// <returns>True if url was tested</returns>
        [HttpPost("test/")]
        public async Task<ActionResult<bool>> TestWebsite([FromBody] TestUrlRequest url)
        {
            var result = await _resultsSaverService.GetAndSaveResultsAsync(System.Web.HttpUtility.UrlDecode(url.Url));
            return result.ToApiResponseResult();
        }

    }
}
