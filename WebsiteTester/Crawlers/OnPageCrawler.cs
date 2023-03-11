using Microsoft.VisualBasic;
using System.Net;
using System.Xml;
using WebsiteTester.Interfaces;

namespace WebsiteTester.Crawlers
{
    public class OnPageCrawler : IPageCrawler
    {
        public IEnumerable<string> Crawl(string url)
        {
            //may be use lib
            var client = new WebClient();
            var html = client.DownloadString(url);
            var startIndex = 0;
            while ((startIndex = html.IndexOf("<a ", startIndex)) != -1)
            {
                var hrefIndex = html.IndexOf("href=", startIndex);
                if (hrefIndex == -1) break;

                var linkStartIndex = hrefIndex + 6;
                var linkEndIndex = html.IndexOf('"', linkStartIndex);
                if (linkEndIndex == -1) break;

                var link = html.Substring(linkStartIndex, linkEndIndex - linkStartIndex);
                yield return link;

                startIndex = linkEndIndex;
            }

        }
    }
}