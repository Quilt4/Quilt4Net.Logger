using Quilt4Net.Dtos;

namespace Quilt4Net;

public static class ChannelFilterExtensions
{
    public static bool ShouldLog(this IChannelFilter filter, int logLevel)
    {
        if (filter?.LogLevel != null && logLevel < filter.LogLevel)
        {
            return false;
        }

        return true;
    }
}