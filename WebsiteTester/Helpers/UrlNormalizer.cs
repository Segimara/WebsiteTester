using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteTester.Helpers
{
    public class UrlNormalizer
    {
        public IEnumerable<string> NormalizeUrls(IEnumerable<string> urls, string baseUrl)
        {
            Uri baseUri = new Uri(baseUrl, UriKind.Absolute);
            return urls.Select(s => StringToUri(s, baseUri))
                .Where(u => u != null && u.Host == baseUri.Host)
                .Select(u => GetNormalizedUri(u));
        }

        private Uri StringToUri(string url, Uri baseUri)
        {
            Uri uri = null;

            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                return uri;
            }
            if (Uri.TryCreate(baseUri, url, out uri))
            {
                return uri;
            }

            return uri;
        }

        private string GetNormalizedUri(Uri url)
        {
            string normalizedUrl = url.GetLeftPart(UriPartial.Path);
            if (normalizedUrl.EndsWith("/"))
            {
                normalizedUrl = normalizedUrl.Substring(0, normalizedUrl.Length - 1);
            }
            return normalizedUrl;
        }
    }
}
