using WebsiteTester.Normalizers;
using Xunit;

namespace WebsiteTester.Tests.Normalizers
{
    public class UrlNormalizerTests
    {
        private readonly UrlNormalizer _urlNormalizer;

        public UrlNormalizerTests()
        {
            _urlNormalizer = new UrlNormalizer();
        }

        [Fact]
        public void NormalizeUrls_WhenUrlsIsNull_ShouldReturnEmptyList()
        {
            var result = _urlNormalizer.NormalizeUrls(null, "http://www.google.com");

            Assert.Empty(result);
        }

        [Fact]
        public void NormalizeUrls_WhenUrlsIsEmpty_ShouldReturnEmptyList()
        {
            var result = _urlNormalizer.NormalizeUrls(Enumerable.Empty<string>(), "http://www.google.com");

            Assert.Empty(result);
        }

        [Fact]
        public void NormalizeUrls_WhenBaseUrlIsNull_ShouldReturnEmptyList()
        {
            var result = _urlNormalizer.NormalizeUrls(new[] { "http://www.google.com" }, null);

            Assert.Empty(result);
        }

        [Fact]
        public void NormalizeUrls_ListWithUrlsEndsWithBackspace_ShouldReturnListWithoutBackspace()
        {
            var result = _urlNormalizer.NormalizeUrls(new[] { "http://www.google.com/" }, "http://www.google.com");

            Assert.Equal("http://www.google.com", result.First());
        }

        [Fact]
        public void NormalizeUrls_ListWithRelativeUrls_ShouldReturnListOfAbsoluteUrls()
        {
            var result = _urlNormalizer.NormalizeUrls(new[] { "/test" }, "http://www.google.com");


            Assert.Equal("http://www.google.com/test", result.First());
        }
    }
}
