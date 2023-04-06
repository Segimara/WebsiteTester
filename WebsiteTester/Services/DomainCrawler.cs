using WebsiteTester.Models;

namespace WebsiteTester.Services
{
    public class DomainCrawler
    {
        private readonly PageLinkFinder _linkFinder;
        private readonly WebPageTester _webTester;

        public DomainCrawler(PageLinkFinder linkFinder, WebPageTester webPageTester)
        {
            _linkFinder = linkFinder;
            _webTester = webPageTester;
        }

        public IEnumerable<WebLinkModel> GetUrls(string url)
        {
            var linksFromUrl = _linkFinder.Extract(url);

            return _webTester.Test(linksFromUrl);
        }
    }
}
