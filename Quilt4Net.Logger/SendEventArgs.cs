namespace Quilt4Net;

public class SendEventArgs : EventArgs
{
    public SendEventArgs(ESendAction sendAction, int sendCount, string message)
    {
        SendAction = sendAction;
        SendCount = sendCount;
        Message = message;
    }

    public ESendAction SendAction { get; }
    public int SendCount { get; }
    public string Message { get; }
}