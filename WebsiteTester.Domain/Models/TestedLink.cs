namespace WebsiteTester.Domain.Models
{
    public class TestedLink
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Url { get; set; }
        public ICollection<LinkTestResult> Links { get; set; }

        public TestedLink()
        {
            Links = new List<LinkTestResult>();
        }
    }
}
