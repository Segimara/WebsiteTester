using Microsoft.AspNetCore.Mvc;
using WebsiteTester.Crawlers;
using WebsiteTester.Domain.Validators;
using WebsiteTester.MVC.Logic.Services;

namespace WebsiteTester.MVC.Controllers
{
    public class WebsiteTesterController : Controller
    {
        private readonly WebsiteCrawler _websiteCrawler;
        private readonly ResultsSaverService _resultsSaverService;
        private readonly ResultsReceiverService _resultsReceiverService;
        public WebsiteTesterController(ResultsReceiverService resultsReceiverService, ResultsSaverService resultsSaverService, WebsiteCrawler websiteCrawler)
        {
            _websiteCrawler = websiteCrawler;
            _resultsSaverService = resultsSaverService;
            _resultsReceiverService = resultsReceiverService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _resultsReceiverService.GetResultsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> TestUrl([UrlForTestValidation] string url)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", await _resultsReceiverService.GetResultsAsync());
            }

            var links = await _websiteCrawler.GetUrlsAsync(url);

            await _resultsSaverService.SaveResultsAsync(url, links);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            return View(await _resultsReceiverService.GetTestDetailAsync(id));
        }
    }
}
