using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteTester.Validators
{
    public class UrlValidator
    {
        public bool IsValid(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            if (url.StartsWith("/") || url.StartsWith("#"))
            {
                return true;
            }

            var isWebLink = url.StartsWith("http://") || url.StartsWith("https://");

            if (isWebLink)
            {
                return true;
            }

            return false;
        }
    }
}
