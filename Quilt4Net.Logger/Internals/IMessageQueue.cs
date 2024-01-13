using Quilt4Net.Dtos;

namespace Quilt4Net.Internals;

internal interface IMessageQueue
{
    int QueueCount { get; }
    event EventHandler<QueueEventArgs> QueueEvent;
    void Enqueue(LogInput logInput);
    LogInput DequeueOne(CancellationToken cancellationToken);
    void SetConfiguration(Configuration configuration);
}