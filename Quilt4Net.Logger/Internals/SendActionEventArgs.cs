namespace Quilt4Net.Internals;

internal class SendActionEventArgs : EventArgs
{
    public SendActionEventArgs(ESendActionEh sendAction, string message = null, Exception exception = null)
    {
        SendAction = sendAction;
        Message = message;
        Exception = exception;
    }

    public ESendActionEh SendAction { get; }
    public string Message { get; }
    public Exception Exception { get; }
}