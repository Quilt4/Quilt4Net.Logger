using Microsoft.Extensions.Logging;

namespace Quilt4Net.Internals;

internal class Quilt4NetLogger : ILogger
{
    private static ISender _sender;
    private readonly string _categoryName;
    private readonly LogLevel _minLogLevel;
    private readonly LogAppData _appData;

    internal Quilt4NetLogger(ISender sender, IConfigurationDataLoader configurationDataLoader, string categoryName = null)
    {
        _sender = sender;
        _categoryName = categoryName;
        var configuration = configurationDataLoader.Get();
        _minLogLevel = configuration.MinLogLevel;
        _appData = configuration.AppData;
    }


    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var logData = ((IEnumerable<KeyValuePair<string, object>>)state).ToLogData();

        var parsedMessage = formatter.Invoke(state, exception);
        logData.AddField("message", parsedMessage);

        var logEntry = new LogMessage
        {
            Message = parsedMessage,
            Exception = exception,
            LogLevel = logLevel,
            Data = logData
        };

        Send(logEntry);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _minLogLevel;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return new LogScope<TState>(state);
    }

    private void Send(LogMessage logMessage)
    {
        var logDataItems = new List<LogDataItem>();
        try
        {
            logDataItems = logMessage.GetData()
                .Where(x => x.Key != "Message" && $"{x.Value}" != logMessage.Message)
                .Select(ToLogData)
                .ToList();
        }
        catch (Exception e)
        {
            logDataItems.Add(new LogDataItem { Key = "Q.Exception", Value = e.Message, Type = e.Message.GetType().Name });
            logDataItems.Add(new LogDataItem { Key = "Q.StackTrace", Value = e.StackTrace, Type = e.StackTrace?.GetType().Name });
        }

        var logInput = new LogInput
        {
            CategoryName = _categoryName,
            LogLevel = (int)logMessage.LogLevel,
            Message = logMessage.Message,
            AppData = _appData,
            Data = logDataItems.ToArray(),
        };

        _sender.Send(logInput);
    }

    protected  virtual LogDataItem ToLogData(KeyValuePair<string, object> x)
    {
        return x.ToLogDataItem();
    }
}