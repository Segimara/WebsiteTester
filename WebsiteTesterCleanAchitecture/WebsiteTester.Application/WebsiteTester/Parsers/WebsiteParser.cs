using WebsiteTester.Application.WebsiteTester.Interfaces;
using WebsiteTester.Application.WebsiteTester.Normalizers;
using WebsiteTester.Application.WebsiteTester.Validators.Interfaces;

namespace WebsiteTester.Application.WebsiteTester.Parsers;

public class WebsiteParser
{
    private readonly IHttpClientService _contentLoaderService;
    private readonly IUrlNormalizer _urlNormalizer;
    private readonly ISimpleUrlValidator _urlValidator;

    public WebsiteParser(ISimpleUrlValidator urlValidator, IUrlNormalizer urlNormalizer,
        IHttpClientService contentLoaderService)
    {
        _urlValidator = urlValidator;
        _urlNormalizer = urlNormalizer;
        _contentLoaderService = contentLoaderService;
    }

    public virtual IEnumerable<string> Parse(string url)
    {
        var doc = _contentLoaderService.GetAttributeValueOfDescendants(new Uri(url), "href", "a");

        var validatedUrls = doc
            .Where(l => _urlValidator.IsValid(l));

        return _urlNormalizer.NormalizeUrls(validatedUrls, url).Distinct();
    }
}