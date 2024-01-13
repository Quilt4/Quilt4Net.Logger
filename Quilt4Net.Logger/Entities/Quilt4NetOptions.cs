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

    /// <summary>
    /// Provide to have custom logger data. This data will be attached to the session and sent once when the logger is registered the first time.
    /// </summary>
    public ILoggingDefaultData LoggingDefaultData { get; } = new LoggingDefaultData();

    /// <summary>
    /// Overall status for the logger.
    /// </summary>
    public Action<StateChangedEventArgs> LogStateEvent { get; set; }

    /// <summary>
    /// Event on the logger queue.
    /// </summary>
    public Action<QueueEventArgs> QueueEvent { get; set; }

    /// <summary>
    /// Event when a log item is sent to the server.
    /// </summary>
    public Action<SendEventArgs> SendEvent { get; set; }

    /// <summary>
    /// Communication and action events. This can be used for debugging and testing.
    /// </summary>
    [Obsolete("Use LogStateEvent, QueueEvent or SendEvent instead.")]
    public Action<LogEventArgs> LogEvent { get; set; }

    /// <summary>
    /// Can optionally be provided to trigger custom creation of HttpClient. If not provided the logger will create one single HttpClient and use that for the lifetime of the application.
    /// </summary>
    public Func<HttpClient> HttpClientFactory { get; set; }
}