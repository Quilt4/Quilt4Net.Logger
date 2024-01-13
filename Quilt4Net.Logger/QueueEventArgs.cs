namespace Quilt4Net;

public class QueueEventArgs
{
    public QueueEventArgs(EAction action, int queueCount)
    {
        Action = action;
        QueueCount = queueCount;
    }

    public EAction Action { get; }
    public int QueueCount { get; }
}