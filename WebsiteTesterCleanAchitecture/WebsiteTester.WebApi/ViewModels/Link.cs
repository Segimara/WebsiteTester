namespace WebsiteTester.WebApi.ViewModels
{
    public class Link
    {
        public Guid UserId { get; set; }
        public Guid Id { get; init; }
        public string Url { get; init; }
        public DateTimeOffset CreatedOn { get; init; }
        public IEnumerable<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}
