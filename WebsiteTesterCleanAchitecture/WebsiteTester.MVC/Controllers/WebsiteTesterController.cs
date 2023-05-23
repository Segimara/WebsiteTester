using Microsoft.AspNetCore.Mvc;
using WebsiteTester.Application.WebsiteTester.Services;
using WebsiteTester.MVC.Extensions;

namespace WebsiteTester.MVC.Controllers
{
    public class WebsiteTesterController : Controller
    {
        private readonly IResultsSaverService _resultsSaverService;
        private readonly IResultsReceiverService _resultsReceiverService;
        public WebsiteTesterController(IResultsReceiverService resultsReceiverService, IResultsSaverService resultsSaverService)
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
