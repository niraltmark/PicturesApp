using System;
using System.Net;
using System.Text;

namespace SYW.App.Pictures.Domain.WebClient
{
    public interface IWebClientBuilder
    {
        IWebClientBuilder SetTimeOut(int timeout);
        IWebClientBuilder SetReadWriteTimeOut(int timeout);
        IWebClientBuilder SetEncoding(Encoding encoding);
        IWebClientBuilder SetEncoding(Action<WebRequest> customizeRequest);
        IWebClient Create();
    }


    public class WebClientBuilder : IWebClientBuilder
    {
        private int _timeout;
        private int _readwriteTimeout;
        private Encoding _encoding;
        private Action<WebRequest> _customizeRequest;

        public WebClientBuilder()
        {
            // Defaults
            _encoding = Encoding.UTF8;
            _timeout = 30000; // 30 seconds default timeout
            _readwriteTimeout = 5000; // Allow 5 seconds between reads or writes by default
        }

        public IWebClientBuilder SetTimeOut(int timeout)
        {
            _timeout = timeout;
            return this;
        }

        public IWebClientBuilder SetReadWriteTimeOut(int timeout)
        {
            _readwriteTimeout = timeout;
            return this;
        }

        public IWebClientBuilder SetEncoding(Encoding encoding)
        {
            _encoding = encoding;
            return this;
        }

        public IWebClientBuilder SetEncoding(Action<WebRequest> customizeRequest)
        {
            _customizeRequest = customizeRequest;
            return this;
        }

        public IWebClient Create()
        {
            return new WebClient
            {
                RequestTimeout = _timeout,
                CustomizeRequest = _customizeRequest,
                Encoding = _encoding,
                ReadWriteTimeout = _readwriteTimeout
            };
        }
    }
}