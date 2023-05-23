﻿namespace WebsiteTester.Services;

public interface IHttpClientService
{
    IEnumerable<string> GetAttributeValueOfDescendants(Uri uri,
        string attribute,
        string descendant);

    HttpResponseMessage GetContent(Uri uri);

    int GetRenderTime(Uri uri);
}