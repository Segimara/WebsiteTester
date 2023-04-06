using System.Diagnostics;
using WebsiteTester.Models;

namespace WebsiteTester.Services
{
    public class WebPageTester
    {
        public IEnumerable<WebLinkModel> Test(IEnumerable<WebLinkModel> urls)
        {
            return urls
                .Select(url =>
                {
                    url.RenderTime = GetRenderTime(url.Url);
                    return url;
                });
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