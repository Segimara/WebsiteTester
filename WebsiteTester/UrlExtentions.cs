using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteTester
{
    public static class UrlExtentions
    {
        public static IEnumerable<string> GetCorrectUrls(this IEnumerable<string> nonUniqueUrls, Uri baseUri)
        {
            foreach (var url in nonUniqueUrls)
            {
                Uri uri;
                Uri _url = null ;
                if (url.StartsWith("/") || url.StartsWith("."))
                {
                    if (Uri.TryCreate(url, UriKind.Relative, out uri))
                    {
                        _url = new Uri(baseUri, url);
                    }
                }
                else if (url.StartsWith("#") || url == "/")
                {
                    _url = baseUri;
                }
                else
                {
                    _url = new Uri(baseUri, url);
                }
                if (_url != null)
                {
                    if (_url.Host == baseUri.Host)
                    {
                        yield return _url.ToString();
                    }
                }
            }
            yield break;
        }
    }
}
