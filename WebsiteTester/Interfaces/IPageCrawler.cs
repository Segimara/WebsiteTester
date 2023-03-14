using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteTester.Interfaces
{
    public interface IPageCrawler
    {
        IEnumerable<string> Crawl(string url);
    }
}
