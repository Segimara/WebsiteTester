using HtmlAgilityPack;

namespace WebsiteTester.Services;

public class ContentLoaderService
{
    private readonly HtmlWeb _loader;

    public ContentLoaderService(HtmlWeb pageLoader)
    {
        _loader = pageLoader;
    }

    public virtual HtmlDocument Load(Uri uri)
    {
        return _loader.Load(uri.ToString());
    }
}