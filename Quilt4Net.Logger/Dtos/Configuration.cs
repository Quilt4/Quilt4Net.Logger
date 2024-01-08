namespace Quilt4Net.Dtos;

public record Configuration
{
    public string Name { get; init; }
    public ChannelFilter Filter { get; init; }
    public int SendIntervalLimitMilliseconds { get; init; }
}