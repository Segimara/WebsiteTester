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
        private readonly HtmlWeb _web;

        public ContentLoaderService(HtmlWeb web)
        {
            _web = web;
        }

        public virtual HtmlDocument Load(Uri uri)
        {
            return _web.Load(uri);
        }
    }
}
