using System;

namespace Quilt4Net
{
    public class Quilt4NetOptions
    {
        public string ApiKey { get; set; }
        public string BaseAddress { get; set; }
        public ILoggingDefaultData LoggingDefaultData { get; } = new LoggingDefaultData();
        public Action<LogFailEventArgs> LogFailEvent { get; set; }
        public Action<LogCompleteEventArgs> LogCompleteEvent { get; set; }
    }
}