namespace Quilt4Net.Internals;

internal interface IMessageQueue
{
    int QueueCount { get; }
    public event EventHandler<QueueEventArgs> QueueEvent;
    void Enqueue(LogInput logInput);
    LogInput DequeueOne(CancellationToken cancellationToken);
    void SetConfiguration(Configuration configuration);
}