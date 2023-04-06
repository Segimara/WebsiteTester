using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteTester.Services.Validators
{
    public class UrlValidator
    {
        public bool isValid(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            if (url.StartsWith("/"))
            {
                return true;
            }
            if (url.StartsWith("#"))
            {
                return true;
            }
            if (url.StartsWith("http://") || url.StartsWith("https://"))
            {
                return true;
            }
            return false;
        }
    }
}
