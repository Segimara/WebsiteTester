using System;
using System.Diagnostics;
using System.Net;
using WebsiteTester.Crawlers;
using WebsiteTester.Services.Parsers;

namespace WebsiteTester.Services
{
    public class WebPageTester
    {
        public IEnumerable<(string, long)> Test(IEnumerable<string> urls)
        {
            return urls
                .Select(url => (url, GetRenderTime(url)));
        }

        private long GetRenderTime(string url)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        
    }
}