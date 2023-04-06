namespace WebsiteTester.Models
{
    public class WebLinkModel
    {
        public string Url { get; set; }
        public long RenderTime { get; set; }
        public bool IsInSitemap { get; set; }
        public bool IsInWebsite { get; set; }
    }
}
