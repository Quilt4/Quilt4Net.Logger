namespace Quilt4Net.Internals;

internal class QueueEventArgs
{
    public int QueueCount { get; }

    public QueueEventArgs(int queueCount)
    {
        QueueCount = queueCount;
    }
}