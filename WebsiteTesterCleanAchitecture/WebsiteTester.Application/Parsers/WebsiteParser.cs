using WebsiteTester.Normalizers;
using WebsiteTester.Services;
using WebsiteTester.Validators;

namespace WebsiteTester.Parsers;

public class WebsiteParser
{
    private readonly ContentLoaderService _contentLoaderService;
    private readonly UrlNormalizer _urlNormalizer;
    private readonly UrlValidator _urlValidator;

    public WebsiteParser(UrlValidator urlValidator, UrlNormalizer urlNormalizer,
        ContentLoaderService contentLoaderService)
    {
        _urlValidator = urlValidator;
        _urlNormalizer = urlNormalizer;
        _contentLoaderService = contentLoaderService;
    }

    public virtual IEnumerable<string> Parse(string url)
    {
        var doc = _contentLoaderService.Load(new Uri(url));

        if (!doc.DocumentNode.Descendants().Any())
        {
            throw new Exception("it is not a html document");
        }

        var validatedUrls = doc.DocumentNode.Descendants("a")
            .Select(a => a.GetAttributeValue("href", null))
            .Where(l => _urlValidator.IsValid(l));

        return _urlNormalizer.NormalizeUrls(validatedUrls, url).Distinct();
    }
}