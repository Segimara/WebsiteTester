namespace WebsiteTester.Domain.Models
{
    public class Link
    {
        public Link()
        {
            LinkTestResults = new List<LinkTestResult>();
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Url { get; set; }
        public DateTimeOffset CreatedOn { get; set; }

        public ICollection<LinkTestResult> LinkTestResults { get; set; }
    }
}
