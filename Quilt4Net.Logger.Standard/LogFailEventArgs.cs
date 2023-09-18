using System;
using System.Net;

namespace Quilt4Net
{
    public class LogFailEventArgs : EventArgs
    {
        internal LogFailEventArgs(LogInput logInput, HttpStatusCode? statusCode, string reasonPhrase, TimeSpan elapsed)
        {
            LogInput = logInput;
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            Elapsed = elapsed;
        }

        public LogInput LogInput { get; }
        public HttpStatusCode? StatusCode { get; }
        public string ReasonPhrase { get; }
        public TimeSpan Elapsed { get; }
    }
}