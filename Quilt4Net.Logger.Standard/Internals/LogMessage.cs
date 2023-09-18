using System;
using Microsoft.Extensions.Logging;

namespace Quilt4Net.Internals
{
    internal class LogMessage
    {
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public LogLevel LogLevel { get; set; }
        public LogData Data { get; set; }
    }
}