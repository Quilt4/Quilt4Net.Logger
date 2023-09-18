namespace Quilt4Net;

public record LogSessionData
{
    public DateTime? ClientTime { get; init; }
    public string IpAddress { get; init; }
    public string CurrentUser { get; init; }
}