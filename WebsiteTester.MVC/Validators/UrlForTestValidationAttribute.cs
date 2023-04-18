using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WebsiteTester.Domain.Validators
{
    public class UrlForTestValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var url = value as string;

            if (url == null)
            {
                return new ValidationResult("The URL is required.");
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                return new ValidationResult("The URL is not valid.");
            }

            try
            {
                var request = WebRequest.Create(uri);
                request.Method = "HEAD";
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return new ValidationResult("The URL returned a non-success status code.");
                    }
                }
            }
            catch (WebException)
            {
                return new ValidationResult("The URL could not be accessed.");
            }

            return ValidationResult.Success;
        }
    }
}
