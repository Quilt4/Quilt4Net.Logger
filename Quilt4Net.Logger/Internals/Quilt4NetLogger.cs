using Microsoft.Extensions.Logging;

namespace Quilt4Net.Internals;

internal class Quilt4NetLogger : ILogger
{
    private readonly IMessageQueue _messageQueue;
    private readonly string _categoryName;
    private readonly LogLevel _minLogLevel;
    //private readonly LogAppData _appData;
    //private readonly LogSessionData _sessionData;

    internal Quilt4NetLogger(IMessageQueue messageQueue, IConfigurationDataLoader configurationDataLoader, string categoryName = null)
    {
        _messageQueue = messageQueue;
        _categoryName = categoryName;
        var configuration = configurationDataLoader.Get();
        _minLogLevel = configuration.MinLogLevel;
        //_appData = configuration.AppData;
        //_sessionData = configuration.SessionData;
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

        var dateTime = DateTime.UtcNow;
        var logInput = new LogInput
        {
            CategoryName = _categoryName,
            LogLevel = (int)logMessage.LogLevel,
            Message = logMessage.Message,
            Data = logDataItems.ToArray(),
            TimeInTicks = dateTime.Ticks
        };

        _messageQueue.Enqueue(logInput);
    }

    protected  virtual LogDataItem ToLogData(KeyValuePair<string, object> x)
    {
        return x.ToLogDataItem();
    }
}