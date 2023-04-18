namespace WebsiteTester.MVC.Models
{
    public class TestDetail
    {
        public string TestedUrl { get; set; }

        public IEnumerable<TestResult> TestResults { get; set; }
    }
}
