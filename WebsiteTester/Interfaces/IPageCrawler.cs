using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteTester.Interfaces
{
    public interface IPageCrawler
    {
        int Counter { get; set; }
        IEnumerable<string> Crawl(string url);
    }
}
