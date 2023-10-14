using System.Diagnostics;

namespace Quilt4Net.Internals;

internal static class StopwatchExtensions
{
    public static TimeSpan StopAndGetElapsed(this Stopwatch stopwatch)
    {
        if (stopwatch == null) return TimeSpan.Zero;
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    public static TimeSpan GetElapsedAndReset(this Stopwatch stopwatch)
    {
        if (stopwatch == null) return TimeSpan.Zero;
        var e = stopwatch.Elapsed;
        stopwatch.Reset();
        stopwatch.Start();
        return e;
    }
}