namespace WebsiteTester.Domain
{
    public class TestedLink
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public IEnumerable<LinkTestResult> Links { get; set; }
    }
}
