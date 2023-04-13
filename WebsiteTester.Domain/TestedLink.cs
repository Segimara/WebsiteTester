namespace WebsiteTester.Domain
{
    public class TestedLink
    {
        public string Url { get; set; }
        public ICollection<LinkTestResult> Links { get; set; }
    }
}
