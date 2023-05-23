namespace WebsiteTester.Application.WebsiteTester.Normalizers;

public class UrlNormalizer : IUrlNormalizer
{
    public IEnumerable<string> NormalizeUrls(IEnumerable<string> urls, string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl) || urls == null)
        {
            return Enumerable.Empty<string>();
        }

        var baseUri = new Uri(baseUrl, UriKind.Absolute);

        return urls.Select(s => ConvertStringToUri(s, baseUri))
            .Where(u => u != null)
            .Where(u => u.Host == baseUri.Host)
            .Select(u => GetNormalizedUri(u));
    }

    private Uri ConvertStringToUri(string url, Uri baseUri)
    {
        Uri uri = null;

        if (Uri.TryCreate(url, UriKind.Absolute, out uri)) return uri;

        if (Uri.TryCreate(baseUri, url, out uri)) return uri;

        return uri;
    }

    private string GetNormalizedUri(Uri url)
    {
        var normalizedUrl = url.GetLeftPart(UriPartial.Path);

        if (normalizedUrl.EndsWith("/")) normalizedUrl = normalizedUrl.Substring(0, normalizedUrl.Length - 1);

        return normalizedUrl;
    }
}