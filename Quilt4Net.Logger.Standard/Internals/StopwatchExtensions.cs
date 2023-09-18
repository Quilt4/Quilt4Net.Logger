using System;
using System.Diagnostics;

namespace Quilt4Net.Internals
{
    internal static class StopwatchExtensions
    {
        public static TimeSpan StopAndGetElapsed(this Stopwatch stopwatch)
        {
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}