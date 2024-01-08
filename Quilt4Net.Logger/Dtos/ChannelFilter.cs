namespace Quilt4Net.Dtos;

public record ChannelFilter : IChannelFilter
{
    public int LogLevel { get; init; }
}