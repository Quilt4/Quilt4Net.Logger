namespace Quilt4Net;

public record ChannelFilter : IChannelFilter
{
    public int LogLevel { get; init; }
}