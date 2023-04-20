namespace WebsiteTester.MVC.Logic.Models
{
    public class Link
    {
        public Link()
        {
            TestResults = new List<TestResult>();
        }

        public Guid Id { get; init; }
        public string Url { get; init; }
        public DateTimeOffset CreatedOn { get; init; }
        public IEnumerable<TestResult> TestResults { get; set; }
    }
}
