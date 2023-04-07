using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteTester.Services
{
    public class ContentLoaderService
    {
        private readonly HtmlWeb _loader;

        public ContentLoaderService(HtmlWeb pageLoader)
        {
            _loader = pageLoader;
        }

        public virtual HtmlDocument Load(Uri uri)
        {
            return _loader.Load(uri);
        }
    }
}
