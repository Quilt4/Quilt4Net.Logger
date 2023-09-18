using Microsoft.Extensions.Logging;

namespace Quilt4Net;

internal record ConfigurationData
{
    public string BaseAddress { get; init; }
    public string ApiKey { get; init; }
    public LogLevel MinLogLevel { get; init; }
    public LogAppData AppData { get; init; }
    public Action<LogCompleteEventArgs> LogCompleteEvent { get; init; }
    public Action<LogFailEventArgs> LogFailEvent { get; init; }
}