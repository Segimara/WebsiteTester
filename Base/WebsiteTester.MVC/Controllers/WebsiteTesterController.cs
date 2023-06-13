using Microsoft.AspNetCore.Mvc;
using WebsiteTester.MVC.Extensions;
using WebsiteTester.Web.Logic.Services;

namespace WebsiteTester.MVC.Controllers
{
    public class WebsiteTesterController : Controller
    {
        private readonly ResultsSaverService _resultsSaverService;
        private readonly ResultsReceiverService _resultsReceiverService;
        public WebsiteTesterController(ResultsReceiverService resultsReceiverService, ResultsSaverService resultsSaverService)
        {
            _resultsSaverService = resultsSaverService;
            _resultsReceiverService = resultsReceiverService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _resultsReceiverService.GetResultsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> TestUrl(string url)
        {
            var result = await _resultsSaverService.GetAndSaveResultsAsync(url);

            return result.ToRedirectResult(TempData, "Index", "WebsiteTester");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var result = await _resultsReceiverService.GetTestDetailAsync(id);

            return result.ToViewResult(TempData, ViewData, "Details");
        }
    }
}
