using System;
using System.Net.Http;

namespace Quilt4Net.Core.Interfaces
{
    public class WebApiResponseEventArgs : EventArgs
    {
        internal WebApiResponseEventArgs(WebApiRequestEventArgs request, Exception exception)
        {
            Elapsed = request.SetComplete();
            Request = request;
            Exception = exception;
        }

        internal WebApiResponseEventArgs(WebApiRequestEventArgs request, HttpResponseMessage response, string contentData = null)
        {
            Elapsed = request.SetComplete();
            Request = request;
            Response = response;
            ContentData = contentData;
        }

        public bool IsSuccess => Exception == null;
        public WebApiRequestEventArgs Request { get; }
        public Exception Exception { get; }
        public HttpResponseMessage Response { get; }
        public string ContentData { get; }
        public TimeSpan Elapsed { get; private set; }
    }
}