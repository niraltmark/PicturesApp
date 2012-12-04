using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace SYW.App.Pictures.Domain.WebClient
{
    /// <summary>
    /// Provides a version of WebClient that supports additional features.
    /// </summary>
    /// <remarks>
    /// This class supports request timeouts and automatic GZIP decompression for HTTP requests.
    /// For further customizations, use the <see cref="CustomizeRequest"/> event.
    /// </remarks>
    public class WebClient : IWebClient
    {
        public WebClient()
        {
            // This is both here and on the WebClientBuilder since some usages create the instance directly.
            Encoding = Encoding.UTF8;
            RequestTimeout = 30000; // 30 seconds default timeout
            ReadWriteTimeout = 5000; // Allow 5 seconds between reads or writes by default.
        }

        /// <summary>
        /// Gets or sets the time in milliseconds to wait for a response. The default is 30 seconds.
        /// </summary>
        /// <remarks>
        /// Be advised that this value only controls the timeout for the request to be answered with a response stream; 
        /// it does not guarantee that reading the content of the response would finish in the allotted time! 
        /// Calling a download method that reads the response in its entirety will NOT time out if the server sends the first byte
        /// of the response in less than this value, regardless of how long it takes to read the full response.
        /// </remarks>
        public int RequestTimeout { get; set; }

        /// <summary>
        /// Gets or sets the time in milliseconds to wait between individual reads from the response stream 
        /// or individual writes to the request stream.
        /// </summary>
        /// <remarks>
        /// Be advised that this timeout applies to individual reads or writes, and not to the process of reading 
        /// the entire response or writing the entire request. 
        /// Calling a download method that reads the response in its entirety will NOT time out if the delays between 
        /// each byte of the response are less than this value, regardless of how long it takes to read the full response.
        /// </remarks>
        public int ReadWriteTimeout { get; set; }

        /// <summary>
        /// The encoding used to encode the request and decode the response. The default is UTF8.
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Allows further customization of the request before it is executed.
        /// </summary>
        public Action<WebRequest> CustomizeRequest;

        public string UploadValues(Uri uri, string method, NameValueCollection data)
        {
            return UploadValues(uri, method, UrlEncode(data));
        }

        public string UploadValues(Uri uri, string method, string data)
        {
            method = method.ToUpper();
            switch (method)
            {
                case "POST":
                case "PUT":
                    return PostRequest(uri, method, data);

                case "GET":
                    return GetRequest(uri, method, data);
                default:
                    throw new NotSupportedException("Supported methods include GET/PUT/POST while the method has been invoked with: " + method);
            }
        }

        private string GetRequest(Uri uri, string method, string data)
        {
            uri = new Uri(uri + "?" + data);
            var request = GetWebRequest(uri);
            request.Method = method;
            return ReadResponse(request);
        }

        private string PostRequest(Uri uri, string method, string data)
        {
            var request = GetWebRequest(uri);
            request.Method = method;

            if (method == "POST")
                request.ContentType = "application/x-www-form-urlencoded";

            var bytes = Encoding.GetBytes(data);
            request.ContentLength = bytes.Length;
            using (var dataStream = request.GetRequestStream())
                dataStream.Write(bytes, 0, bytes.Length);
            return ReadResponse(request);
        }

        public string DownloadString(string url)
        {
            return DownloadString(new Uri(url));
        }

        public string DownloadString(Uri uri)
        {
            var request = GetWebRequest(uri);
            request.Method = "GET";
            return ReadResponse(request);
        }

        public T GetJson<T>(Uri url)
        {
            var response = DownloadString(url);
            return JsonConvert.DeserializeObject<T>(response);
        }

        public T GetJson<T>(Uri url, NameValueCollection data, string method = "GET")
        {
            var response = UploadValues(url, method, data);
            return JsonConvert.DeserializeObject<T>(response);
        }

        private string UrlEncode(NameValueCollection data)
        {
            return data.AllKeys
                .Aggregate(new StringBuilder(), (sb, k) => sb
					.Append(HttpUtility.UrlEncode(k, Encoding))
					.Append("=")
					.Append(HttpUtility.UrlEncode(data[k], Encoding))
					.Append("&")
                )
                .ToString();
        }

        private string ReadResponse(WebRequest request)
        {
            using (var response = request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding))
            {
                return reader.ReadToEnd();
            }
        }

        private WebRequest GetWebRequest(Uri url)
        {
            var request = WebRequest.Create(url);
            CustomizeRequestInternal(request);
            return request;
        }

        private void CustomizeRequestInternal(WebRequest wr)
        {
            wr.Timeout = RequestTimeout;

            var hwr = wr as HttpWebRequest;
            if (hwr != null)
            {
                hwr.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                hwr.ReadWriteTimeout = ReadWriteTimeout;
            }

            if (CustomizeRequest != null) CustomizeRequest(wr);
        }
    }
}