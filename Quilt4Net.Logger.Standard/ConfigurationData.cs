using Microsoft.Extensions.Logging;
using System;

namespace Quilt4Net
{
    public class ConfigurationData
    {
        public string BaseAddress { get; set; }
        public string ApiKey { get; set; }
        public LogLevel MinLogLevel { get; set; }
        public LogAppData AppData { get; set; }
        public Action<LogCompleteEventArgs> LogCompleteEvent { get; set; }
        public Action<LogFailEventArgs> LogFailEvent { get; set; }
    }
}