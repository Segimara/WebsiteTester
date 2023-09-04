using WebsiteTester.Crawler.Validators.Interfaces;

namespace WebsiteTester.Crawler.Validators;

public class SimpleUrlValidator : ISimpleUrlValidator
{
    public virtual bool IsValid(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return false;
        }

        var isWebLink = url.StartsWith("http://") || url.StartsWith("https://");

        var isLocalLink = url.StartsWith("/");

        var isLinkWithParameterOrSections = url.Contains("?") || url.Contains("#");

        return (isLocalLink || isWebLink) && !isLinkWithParameterOrSections;
    }
}