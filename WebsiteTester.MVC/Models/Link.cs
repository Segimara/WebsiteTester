namespace WebsiteTester.MVC.Models
{
    public class Link
    {
        public Guid Id { get; init; }
        public string Url { get; init; }
        public DateTimeOffset CreatedOn { get; init; }

    }
}
