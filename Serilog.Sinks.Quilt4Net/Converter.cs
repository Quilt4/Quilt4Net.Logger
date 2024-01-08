using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace Serilog.Sinks.Quilt4Net;

internal static class Converter
{
    public static LogLevel ToLogLevel(this LogEventLevel logEventLevel)
    {
        switch (logEventLevel)
        {
            case LogEventLevel.Verbose:
                return LogLevel.Trace;
            case LogEventLevel.Debug:
                return LogLevel.Debug;
            case LogEventLevel.Information:
                return LogLevel.Information;
            case LogEventLevel.Warning:
                return LogLevel.Warning;
            case LogEventLevel.Error:
                return LogLevel.Error;
            case LogEventLevel.Fatal:
                return LogLevel.Critical;
            default:
                Debugger.Break();
                throw new ArgumentOutOfRangeException($"Unknown log even level '{logEventLevel}'.");
        }
    }
}