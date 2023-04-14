namespace WebsiteTester.Domain.Models
{
    public class LinkTestResult
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTimeOffset CreatedOn { get; set; }
        public string Url { get; set; }
        public int RenderTimeMilliseconds { get; set; }
        public bool IsInSitemap { get; set; }
        public bool IsInWebsite { get; set; }

        public Guid TestedLinkId { get; set; }
        public TestedLink TestedLink { get; set; }
    }
}