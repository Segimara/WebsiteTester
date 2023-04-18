using Microsoft.AspNetCore.Mvc;
using WebsiteTester.Crawlers;
using WebsiteTester.MVC.Services;
using WebsiteTester.Persistenсe;

namespace WebsiteTester.MVC.Controllers
{
    public class WebsiteTesterController : Controller
    {
        private readonly WebsiteCrawler _websiteCrawler;
        private readonly WebsiteTesterDbContext _dbContext;
        private readonly ResultsSaverService _resultsSaverService;
        private readonly ResultsReceiverService _resultsReceiverService;
        public WebsiteTesterController(ResultsReceiverService resultsReceiverService, ResultsSaverService resultsSaverService, WebsiteCrawler websiteCrawler, WebsiteTesterDbContext dbContext)
        {
            _websiteCrawler = websiteCrawler;
            _dbContext = dbContext;
            _resultsSaverService = resultsSaverService;
            _resultsReceiverService = resultsReceiverService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _resultsReceiverService.GetResultsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Index(string url)
        {
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
