using Microsoft.AspNetCore.Mvc;
using WebsiteTester.Application.Features.WebsiteTester.DtoModels;
using WebsiteTester.Application.Features.WebsiteTester.Services;
using WebsiteTester.WebApi.Extensions;
using WebsiteTester.WebApi.Models;

namespace WebsiteTester.WebApi.Controllers
{
    public class TestsController : BaseController
    {
        private readonly IResultsSaverService _resultsSaverService;
        private readonly IResultsReceiverService _resultsReceiverService;
        public TestsController(IResultsReceiverService resultsReceiverService, IResultsSaverService resultsSaverService)
        {
            _resultsSaverService = resultsSaverService;
            _resultsReceiverService = resultsReceiverService;
        }

        /// <summary>
        /// Action for get list of tested liks with out details
        /// </summary>
        /// <returns>Links Array</returns>
        /// <response code = "200">Success</response>
        /// <response code = "500">If something went wrong</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ViewModels.Link>>> TestResults()
        {
            var result = await _resultsReceiverService.GetResultsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Action for get test results 
        /// </summary>
        /// <param name="id">Id of link</param>
        /// <returns>Link model with </returns>
        /// <response code = "200">Success</response>
        /// <response code = "400">If given id is not valid</response>
        /// <response code = "500">If something went wrong</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ViewModels.Link>> TestDetails(Guid id)
        {
            var result = await _resultsReceiverService.GetTestDetailAsync(id.ToString());
            var mapper = delegate (Link link)
            {
                return new ViewModels.Link
                {
                    CreatedOn = link.CreatedOn,
                    Id = link.Id,
                    Url = link.Url,
                    TestResults = link.TestResults.Select(l => new ViewModels.TestResult
                    {
                        CreatedOn = l.CreatedOn,
                        Url = l.Url,
                        IsInSitemap = l.IsInSitemap,
                        IsInWebsite = l.IsInWebsite,
                        RenderTimeMilliseconds = l.RenderTimeMilliseconds,
                    }),
                };
            };
            return result.ToApiResponseResult(mapper);
        }

        /// <summary>
        /// Action runs test for given url
        /// </summary>
        /// <param name="url">Url for performance testing </param>
        /// <returns>True if url was tested</returns>
        /// <response code = "200">Success</response>
        /// <response code = "400">If given url is not valid</response>
        /// <response code = "500">If something went wrong</response>
        [HttpPost("test")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> TestWebsite([FromBody] TestUrlRequest url)
        {
            var result = await _resultsSaverService.GetAndSaveResultsAsync(System.Web.HttpUtility.UrlDecode(url.Url));

            return result.ToApiResponseResult(b => b);
        }

    }
}
