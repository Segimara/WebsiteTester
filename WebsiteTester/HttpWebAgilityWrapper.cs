using HtmlAgilityPack;

namespace WebsiteTester
{
    public class HttpWebAgilityWrapper
    {
        private readonly HtmlWeb _web;

        public HttpWebAgilityWrapper()
        {
            _web = new HtmlWeb();
        }

        public virtual HtmlDocument Load(Uri uri)
        {
            return _web.Load(uri);
        }
    }
}
