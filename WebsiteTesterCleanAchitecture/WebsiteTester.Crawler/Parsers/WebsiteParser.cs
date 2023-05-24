using WebsiteTester.Crawler.Normalizers;
using WebsiteTester.Crawler.Services;
using WebsiteTester.Crawler.Validators.Interfaces;

namespace WebsiteTester.Crawler.Parsers;

public class WebsiteParser
{
    private readonly HttpClientService _contentLoaderService;
    private readonly IUrlNormalizer _urlNormalizer;
    private readonly ISimpleUrlValidator _urlValidator;

    public WebsiteParser(ISimpleUrlValidator urlValidator, IUrlNormalizer urlNormalizer,
        HttpClientService contentLoaderService)
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