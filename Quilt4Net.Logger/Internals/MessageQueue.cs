using System.Collections.Concurrent;
using System.Diagnostics;

namespace Quilt4Net.Internals;

internal class MessageQueue : IMessageQueue
{
    private readonly BlockingCollection<LogInput> _queue = new();
    //private readonly Queue<LogInput> _queue = new();

    public MessageQueue()
    {
    }

    public void Enqueue(LogInput logInput)
    {
        //TODO: Check if the queue is full. Prioritize issues with higher level and remove older messages first.
        _queue.Add(logInput);
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

    public int QueueCount => _queue.Count;
}