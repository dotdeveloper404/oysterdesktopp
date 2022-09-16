namespace OysterVPNLibrary
{
    using System;
    using System.Collections.Specialized;

    public interface IHttpClient
    {
        string GetData(Uri url);
        string PostData(string url, NameValueCollection data);
    }
}

