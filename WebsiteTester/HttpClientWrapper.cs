namespace WebsiteTester
{
    public class HttpClientWrapper : IDisposable
    {
        private volatile bool _disposed;
        HttpClient _client;

        public HttpClientWrapper()
        {
            _client = new HttpClient();
        }

        public virtual async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            return await _client.GetAsync(uri);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _client.Dispose();
            }

            _disposed = true;
        }
    }
}
