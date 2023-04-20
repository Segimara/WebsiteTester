using System.ComponentModel.DataAnnotations;
using WebsiteTester.MVC.Domain.Validators;

namespace WebsiteTester.MVC.Domain.Models
{
    public class UrlForTest
    {
        [Required(ErrorMessage = "The URL is required.")]
        [Url]
        [UrlValidation]
        public string Url { get; set; }
    }
}
