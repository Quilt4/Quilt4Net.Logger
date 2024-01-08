using Quilt4Net.Internals;

namespace Quilt4Net.Entities;

public record Quilt4NetOptions
{
    public string ApiKey { get; set; }
    public string BaseAddress { get; set; }
    public ILoggingDefaultData LoggingDefaultData { get; } = new LoggingDefaultData();
    public Action<LogEventArgs> LogEvent { get; set; }
    public Func<HttpClient> HttpClientFactory { get; set; }
}