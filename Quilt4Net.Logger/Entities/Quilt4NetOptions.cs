using Quilt4Net.Internals;

namespace Quilt4Net.Entities;

public record Quilt4NetOptions
{
    /// <summary>
    /// Programmatically set the ApiKey.
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// Programmatically set the address to override the target to send the data to.
    /// </summary>
    public string BaseAddress { get; set; }

    /// <summary>
    /// Programmatically override the Environment name.
    /// </summary>
    public string Environment { get; set; }

    public ILoggingDefaultData LoggingDefaultData { get; } = new LoggingDefaultData();
    public Action<LogEventArgs> LogEvent { get; set; }
    public Func<HttpClient> HttpClientFactory { get; set; }
}