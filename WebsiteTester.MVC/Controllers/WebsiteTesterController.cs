using Microsoft.AspNetCore.Mvc;
using WebsiteTester.Crawlers;
using WebsiteTester.MVC.Domain.Models;
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
            var indexModel = new IndexModel
            {
                Links = await _resultsReceiverService.GetResultsAsync(),
                UrlForTest = new UrlForTest()
            };
            return View(indexModel);
        }

        [HttpPost]
        public async Task<IActionResult> TestUrl(UrlForTest url)
        {
            if (!ModelState.IsValid)
            {
                var indexModel = new IndexModel
                {
                    Links = await _resultsReceiverService.GetResultsAsync(),
                    UrlForTest = new UrlForTest()
                };
                return View("Index", indexModel);
            }
            _resultsSaverService.GetAndSaveResultsAsync(url.Url);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            return View(await _resultsReceiverService.GetTestDetailAsync(id));
        }
    }
}
