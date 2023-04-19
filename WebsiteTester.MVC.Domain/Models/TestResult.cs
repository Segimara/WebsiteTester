namespace WebsiteTester.MVC.Domain.Models
{
    public class TestResult
    {
        public DateTimeOffset CreatedOn { get; init; }
        public string Url { get; init; }
        public int RenderTimeMilliseconds { get; init; }
        public bool IsInSitemap { get; init; }
        public bool IsInWebsite { get; init; }
    }
}
