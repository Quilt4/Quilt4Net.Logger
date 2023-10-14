namespace Quilt4Net.Internals;

public class QueueEventArgs
{
    public int QueueCount { get; }

    public QueueEventArgs(int queueCount)
    {
        QueueCount = queueCount;
    }
}