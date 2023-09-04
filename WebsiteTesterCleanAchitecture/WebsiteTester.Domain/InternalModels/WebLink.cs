namespace WebsiteTester.Domain.InternalModels;

public class WebLink
{
    public string Url { get; set; }
    public int RenderTimeMilliseconds { get; set; }
    public bool IsInSitemap { get; set; }
    public bool IsInWebsite { get; set; }
}