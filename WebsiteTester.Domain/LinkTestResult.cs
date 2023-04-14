namespace WebsiteTester.Domain
{
    public class LinkTestResult
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string Url { get; set; }
        public int RenderTimeMilliseconds { get; set; }
        public bool IsInSitemap { get; set; }
        public bool IsInWebsite { get; set; }

        public string TestedLinkId { get; set; }
        public TestedLink TestedLink { get; set; }
    }
}