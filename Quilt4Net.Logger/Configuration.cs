namespace Quilt4Net;

public record Configuration
{
    public string Name { get; init; }
    public ChannelFilter Filter { get; init; }
    public int SendIntervalLimitMilliseconds { get; init; }
}