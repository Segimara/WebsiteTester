namespace WebsiteTester.Application.WebsiteTester.Normalizers
{
    public interface IUrlNormalizer
    {
        IEnumerable<string> NormalizeUrls(IEnumerable<string> urls, string baseUrl);
    }
}
