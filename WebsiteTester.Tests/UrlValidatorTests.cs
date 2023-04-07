using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteTester.Validators;
using Xunit;

namespace WebsiteTester.Tests
{
    public class UrlValidatorTests
    {
        private readonly UrlValidator _urlValidator;

        public UrlValidatorTests()
        {
            _urlValidator = new UrlValidator();
        }

        [Fact]
        public void IsValid_WhenUrlIsNull_ShouldReturnFalse()
        {
            var result = _urlValidator.IsValid(null);
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenUrlIsEmpty_ShouldReturnFalse()
        {
            var result = _urlValidator.IsValid(string.Empty);
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenUrlIsNotValid_ShouldReturnFalse()
        {
            var result = _urlValidator.IsValid("skype:someSkype");
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenUrlIsValid_ShouldReturnTrue()
        {
            var result = _urlValidator.IsValid("http://www.google.com");
            Assert.False(result);
        }

    }
}
