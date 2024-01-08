using System.Reflection;
using Quilt4Net.Dtos;
using Quilt4Net.Internals;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.Quilt4Net;

internal class Quilt4NetSink : ILogEventSink
{
    private readonly IMessageQueue _messageQueue;

    public Quilt4NetSink(IMessageQueue messageQueue)
    {
        _messageQueue = messageQueue;
    }

    public void Emit(LogEvent logEvent)
    {
        var properties = logEvent.Properties;

        var categoryName = GetCategoryName(properties);

        var logDataItems = properties
            .Where(x => x.Key != "SourceContext")
            .Select(x => new LogDataItem { Key = x.Key, Value = $"{x.Value}" }).ToArray();

        var logInput = new LogInput
        {
            CategoryName = categoryName,
            LogLevel = (int)logEvent.Level.ToLogLevel(),
            Message = logEvent.RenderMessage(),
            Data = logDataItems,
            TimeInTicks = DateTime.UtcNow.Ticks,
        };

        _messageQueue.Enqueue(logInput);
    }

    private static string GetCategoryName(IReadOnlyDictionary<string, LogEventPropertyValue> properties)
    {
        string categoryName;
        if (properties.TryGetValue("SourceContext", out var categoryNameValue) && categoryNameValue is ScalarValue categoryNameScalar)
        {
            categoryName = $"{categoryNameScalar.Value}";
        }
        else
        {
            categoryName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";
        }

        return categoryName;
    }
}