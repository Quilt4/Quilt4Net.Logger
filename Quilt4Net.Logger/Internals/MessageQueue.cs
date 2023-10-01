namespace Quilt4Net.Internals;

internal class MessageQueue : IMessageQueue
{
    private readonly ConfigurationData _configurationData;
    private readonly BlockingLogLevelCollection _queue = new(new LogLevelQueue(1000));
    private Configuration _configuration = new();

    public MessageQueue(IConfigurationDataLoader configurationDataLoader)
    {
        _configurationData = configurationDataLoader.Get();
    }

    public void Enqueue(LogInput logInput)
    {
        if (_configuration?.LogLevel != null && logInput.LogLevel < _configuration.LogLevel)
        {
            _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Warning, logInput, null, $"Skip because only logging {_configuration.LogLevel} and above, this message was {logInput.LogLevel}."));
            return;
        }

        if (_queue.Count != 0 && _queue.Count % 100 == 0)
        {
            _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Warning, null, null, $"Queue is filling up. It contains {QueueCount} items."));
        }

        _queue.TryAdd(logInput);

        _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, $"Added to queue that contains {QueueCount} items."));
    }

    public LogInput DequeueOne(CancellationToken cancellationToken)
    {
        var item = _queue.Take(cancellationToken);
        return item;
    }

    public void SetConfiguration(Configuration configuration)
    {
        _configuration = configuration;
    }

    public int QueueCount => _queue.Count;
}