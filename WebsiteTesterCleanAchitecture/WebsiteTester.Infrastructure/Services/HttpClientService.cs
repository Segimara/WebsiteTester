using HtmlAgilityPack;
using System.Text;
using WebsiteTester.Services;

namespace WebsiteTester.Infrastructure.Services;

public class HttpClientService : IHttpClientService
{
    private readonly HtmlWeb _loader;

    public HttpClientService(HtmlWeb pageLoader)
    {
        _loader = pageLoader;
    }

    public virtual IEnumerable<string> GetAttributeValueOfDescendants(Uri uri,
        string attribute,
        string descendant)
    {
        var doc = _loader.Load(uri);

        if (!doc.DocumentNode.Descendants().Any())
        {
            throw new Exception("it is not a html document");
        }

        return doc.DocumentNode.Descendants(descendant)
            .Select(a => a.GetAttributeValue(attribute, null));
    }

    public HttpResponseMessage GetContent(Uri uri)
    {
        var htmlDocResponse = _loader.Load(uri);

        return ConvertHtmlDocumentToHttpResponseMessage(htmlDocResponse);
    }

    private HttpResponseMessage ConvertHtmlDocumentToHttpResponseMessage(HtmlDocument htmlDocument)
    {
        var stringBuilder = new StringBuilder();

        using (var stringWriter = new System.IO.StringWriter(stringBuilder))
        {
            htmlDocument.Save(stringWriter);
        }

        var htmlContent = stringBuilder.ToString();
        var response = new HttpResponseMessage(_loader.StatusCode);

        response.Content = new StringContent(htmlContent, Encoding.UTF8, "text/html");

        return response;
    }

    public int GetRenderTime(Uri uri)
    {
        _loader.Load(uri);
        return _loader.RequestDuration;
    }
}