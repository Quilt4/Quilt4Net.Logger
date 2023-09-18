using Microsoft.Extensions.Logging;

namespace Quilt4Net.Internals;

internal record LogMessage
{
    public Exception Exception { get; init; }
    public string Message { get; init; }
    public LogLevel LogLevel { get; init; }
    public LogData Data { get; init; }
}