namespace Quilt4Net;

public record LogAppData
{
    public string Environment { get; init; }
    public string Application { get; init; }
    public string Version { get; init; }
    public string Machine { get; init; }
    public string SystemUser { get; init; }
    public LogDataItem[] Data { get; init; }
    public string LoggerInfo { get; init; }
}