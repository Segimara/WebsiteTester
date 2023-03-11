using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteTester.Interfaces
{
    public interface ISiteMapParser
    {
        public IEnumerable<string> Parse(string baseUrl)
        {
            yield return "";
        }
    }
}
