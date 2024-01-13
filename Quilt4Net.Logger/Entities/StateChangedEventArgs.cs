namespace Quilt4Net.Entities;

public class StateChangedEventArgs : EventArgs
{
    public StateChangedEventArgs(ELoggerState state, Exception exception)
    {
        State = state;
        Exception = exception;
    }

    public ELoggerState State { get; }
    public Exception Exception { get; }
}