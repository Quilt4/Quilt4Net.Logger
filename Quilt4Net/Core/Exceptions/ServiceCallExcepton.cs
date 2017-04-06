//using System;
//using System.Net;
//using System.Net.Http;

//namespace Quilt4Net.Core
//{
//    internal class ServiceCallExcepton : InvalidOperationException
//    {
//        public ServiceCallExcepton(string message, HttpResponseMessage httpResponseMessage, Exception innerException = null)
//            : base (message, innerException)
//        {
//            StatusCode = httpResponseMessage.StatusCode;
//        }

//        public HttpStatusCode StatusCode { get; }
//        public override string HelpLink { get { return ""; } }
//    }
//}