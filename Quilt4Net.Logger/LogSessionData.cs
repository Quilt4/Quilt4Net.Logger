namespace Quilt4Net;

//NOTE: One record per start of the application
public record LogSessionData
{
    public string Environment { get; init; }
    public string Machine { get; init; }
    public DateTime? ClientTime { get; init; }
    //public string IpAddress { get; init; }
    public string CurrentUser { get; init; }
    public string SystemUser { get; init; }
    public LogDataItem[] Data { get; init; }
}