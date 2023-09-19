using Quilt4Net.Internals;

namespace Quilt4Net;

public record Quilt4NetOptions
{
    public string ApiKey { get; set; }
    public string BaseAddress { get; set; }
    public ILoggingDefaultData LoggingDefaultData { get; } = new LoggingDefaultData();
    public Func<IServiceProvider, HttpClient> HttpClientLoader { get; set; }
    public Action<LogFailEventArgs> LogFailEvent { get; set; }
    public Action<LogCompleteEventArgs> LogCompleteEvent { get; set; }
}