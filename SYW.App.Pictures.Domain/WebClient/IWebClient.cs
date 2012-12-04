using System;
using System.Collections.Specialized;

namespace SYW.App.Pictures.Domain.WebClient
{
    public interface IWebClient
    {
        string UploadValues(Uri uri, string method, NameValueCollection data);
        string UploadValues(Uri uri, string method, string data);
        string DownloadString(string url);
        string DownloadString(Uri uri);
        T GetJson<T>(Uri url);
		T GetJson<T>(Uri url, NameValueCollection data, string method = "GET");
    }
}