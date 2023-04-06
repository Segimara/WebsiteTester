namespace WebsiteTester
{
    public static class UrlExtentions
    {
        public static IEnumerable<string> NormalizeUrls(this IEnumerable<string> urls, string baseUrl)
        {
            Uri baseUri = new Uri(baseUrl, UriKind.Absolute);
            return urls.Select(s => StringToUri(s, baseUri))
                .Where(u => u != null && u.Host == baseUri.Host)
                .Select(u => GetNormalizedUri(u));
        }

        private static Uri StringToUri(string url, Uri baseUri)
        {
            Uri uri = null;

            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                return uri;
            }
            else if (Uri.TryCreate(baseUri, url, out uri))
            {
                return uri;
            }
            
            return uri;
        }

        private static string GetNormalizedUri(Uri url)
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
