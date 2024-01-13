using Microsoft.Extensions.Logging;
using Quilt4Net.Entities;

namespace Quilt4Net.Dtos;

internal record ConfigurationData : IConfigurationData
{
    public string BaseAddress { get; init; }
    public string ApiKey { get; init; }
    public LogLevel MinLogLevel { get; init; }
    public LogAppData AppData { get; init; }
    public LogSessionData SessionData { get; init; }
    public Action<LogEventArgs> LogEvent { get; init; }
    public Func<HttpClient> HttpClientFactory { get; init; }
}