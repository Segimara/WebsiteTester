using System.ComponentModel.DataAnnotations;
using System.Net;
using WebsiteTester.Application.WebsiteTester.Validators.Interfaces;

namespace WebsiteTester.Application.WebsiteTester.Validators
{
    public class ComplexUrlValidator : IComplexUrlValidator
    {
        public bool IsValid(string url)
        {
            if (url == null)
            {
                throw new ValidationException("The URL is required.");
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                throw new ValidationException("The URL is not valid.");
            }

            try
            {
                var request = WebRequest.Create(uri);
                request.Method = "HEAD";
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new ValidationException("The URL returned a non-success status code.");
                    }
                }
            }
            catch (WebException)
            {
                throw new ValidationException("The URL could not be accessed.");
            }

            return true;
        }
    }
}
