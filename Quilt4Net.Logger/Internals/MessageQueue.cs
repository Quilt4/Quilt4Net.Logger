namespace Quilt4Net.Internals;

internal class MessageQueue : IMessageQueue
{
    private readonly ConfigurationData _configurationData;
    private readonly BlockingLogLevelCollection _queue = new(new LogLevelQueue(1000));
    private Configuration _configuration = new();
    private bool _queueInfoSent;

    public MessageQueue(IConfigurationDataLoader configurationDataLoader)
    {
        _configurationData = configurationDataLoader.Get();
    }

    public event EventHandler<QueueEventArgs> QueueEvent;

    public void Enqueue(LogInput logInput)
    {
        if (!_configuration?.Filter.ShouldLog(logInput.LogLevel) ?? false)
        {
            _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Warning, logInput, null, $"Skip because only logging {_configuration?.Filter.LogLevel} and above, this message was {logInput.LogLevel}."));
            return;
        }

        if (_queue.Count != 0 && _queue.Count % 10 == 0)
        {
            _queueInfoSent = true;
            QueueEvent?.Invoke(this, new QueueEventArgs(QueueCount));
            _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Warning, null, null, $"Queue is filling up. It contains {QueueCount} items."));
        }

        _queue.TryAdd(logInput);

        _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, $"Added to queue that contains {QueueCount} items."));
    }

    public LogInput DequeueOne(CancellationToken cancellationToken)
    {
        var item = _queue.Take(cancellationToken);

        //NOTE: Send information that the queue now is zero again
        if (QueueCount == 0 && _queueInfoSent)
        {
            QueueEvent?.Invoke(this, new QueueEventArgs(0));
            _queueInfoSent = false;
            _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Debug, null, null, "Queue is now empty again."));
        }

        return item;
    }

    public void SetConfiguration(Configuration configuration)
    {
        _configuration = configuration;
    }

    public int QueueCount => _queue.Count;
}