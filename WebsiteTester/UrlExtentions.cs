using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteTester
{
    public static class UrlExtentions
    {
        public static IEnumerable<string> GetCorrectUrls(this IEnumerable<string> urls, string baseUrl)
        {
            Uri baseUri = new Uri(baseUrl, UriKind.Absolute);
            foreach (string url in urls)
            {
                Uri uri;
                if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                {
                    if (uri.Host == baseUri.Host)
                    {
                        yield return GetNormalizedUrl(uri);
                    }
                }
                else if (Uri.TryCreate(baseUri, url, out uri))
                {
                    yield return GetNormalizedUrl(uri);
                }
            }
        }
        private static string GetNormalizedUrl(Uri url)
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
