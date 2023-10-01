using System.Collections.Concurrent;
using System.Diagnostics;

namespace Quilt4Net.Internals;

internal class MessageQueue : IMessageQueue
{
    private readonly ConfigurationData _configurationData;
    private readonly BlockingCollection<LogInput> _queue = new();
    private Configuration _configuration = new ();

    public MessageQueue(IConfigurationDataLoader configurationDataLoader)
    {
        _configurationData = configurationDataLoader.Get();
    }

    public void Enqueue(LogInput logInput)
    {
        if (_configuration?.LogLevel != null && logInput.LogLevel < _configuration.LogLevel)
        {
            _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, $"Skip because only logging {_configuration.LogLevel} and above, this message was {logInput.LogLevel}."));
            return;
        }

        //TODO: Check if the queue is full. Prioritize issues with higher level and remove older messages first.
        _queue.Add(logInput);

        _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, $"Added to queue that contains {QueueCount} items."));
    }

    public LogInput DequeueOne(CancellationToken cancellationToken)
    {
        if (_queue.Count > 100)
        {
            //TODO: Signal that we are running behind
            Debugger.Break();
        }

        var item = _queue.Take(cancellationToken);
        return item;
    }

    public void SetConfiguration(Configuration configuration)
    {
        _configuration = configuration;
    }

    public int QueueCount => _queue.Count;
}