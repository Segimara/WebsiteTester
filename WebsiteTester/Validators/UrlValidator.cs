namespace WebsiteTester.Validators;

public class UrlValidator
{
    public virtual bool IsValid(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return false;
        }

        var isWebLink = url.StartsWith("http://") || url.StartsWith("https://");

        if (url.StartsWith("/") || url.StartsWith("#") || isWebLink)
        {
            return true;
        }

        return false;
    }
}