namespace WebsiteTester.Domain.Models
{
    public class TestedLink
    {
        public TestedLink()
        {
            Links = new List<LinkTestResult>();
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Url { get; set; }
        public ICollection<LinkTestResult> Links { get; set; }
    }
}
