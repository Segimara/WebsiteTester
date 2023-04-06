using System.Diagnostics;
using WebsiteTester.Models;

namespace WebsiteTester.Services
{
    public class WebPageTester
    {
        public IEnumerable<WebLinkModel> Test(IEnumerable<WebLinkModel> urls)
        {
            return urls
                .Select(url => GetRenderTime(url));
        }

        private WebLinkModel GetRenderTime(WebLinkModel url)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url.Url).Result;
            }

            stopwatch.Stop();

            url.RenderTime = stopwatch.ElapsedMilliseconds;

            return url;
        }
        
    }
}