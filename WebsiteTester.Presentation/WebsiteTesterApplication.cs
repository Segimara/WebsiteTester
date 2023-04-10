﻿using WebsiteTester.Crawlers;
using WebsiteTester.Models;
using WebsiteTester.Wrappers;

namespace WebsiteTester.Presentation
{
    public class WebsiteTesterApplication
    {
        private readonly WebsiteCrawler _crawler;
        private readonly ConsoleWrapper _console;

        public WebsiteTesterApplication(WebsiteCrawler crawler, ConsoleWrapper console)
        {
            _crawler = crawler;
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("Enter the website URL: ");

            var url = _console.ReadLine();
            
            GetResults(url);
        }

        private async void GetResults(string url)
        {
            var linksFromUrl = (await _crawler.GetUrls(url));

            var onlyInWebSite = linksFromUrl
                .Where(l => l.IsInWebsite)
                .Where(l => !l.IsInSitemap);

            var onlyInSitemap = linksFromUrl
                .Where(l => l.IsInSitemap)
                .Where(l => !l.IsInWebsite);
            
            OutputUrlsFromPage(onlyInWebSite, onlyInSitemap);

            var results = linksFromUrl
                .OrderBy(x => x.RenderTimeMilliseconds)
                .Select(result => $"{result.Url} \t {result.RenderTimeMilliseconds}");

            OutputList("Timing", results);

            _console.WriteLine($"Urls(html documents) found after crawling a website: {linksFromUrl.Count(u => u.IsInWebsite)}");
            _console.WriteLine($"Urls found in sitemap: {linksFromUrl.Count(u => u.IsInSitemap)}");
        }

        private void OutputUrlsFromPage(IEnumerable<WebLink> onlyInWebSite, IEnumerable<WebLink> onlyInSitemap)
        {
            var messageForUrlsInSitemap = "Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site";
            OutputList(messageForUrlsInSitemap, onlyInSitemap.Select(u => u.Url));

            var messageForUrlsInWebSite = "Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml";
            OutputList(messageForUrlsInWebSite, onlyInWebSite.Select(u => u.Url));
        }

        private void OutputList(string message, IEnumerable<string> urls)
        {
            var i = 1;

            _console.WriteLine(message);

            foreach (var u in urls)
            {
                _console.WriteLine($"{i++}) {u}");
            }
        }
    }
}