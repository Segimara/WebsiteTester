using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteTester.Models
{
    public class WebsiteLinksModel
    {
        public IEnumerable<string> LinksFromPages { get; set; }
        public IEnumerable<string> LinksFromSitemap { get; set; }
        public IEnumerable<string> LinksOnlyInSitemap { get; set; }
        public IEnumerable<string> LinksOnlyInWebsite { get; set; }
        public IEnumerable<string> UniqueLinks { get; set; }
    }
}
