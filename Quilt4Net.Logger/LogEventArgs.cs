using System.Net;

namespace Quilt4Net;

public class LogEventArgs : EventArgs
{
    internal LogEventArgs(ELogState logState, LogInput logInput, HttpStatusCode? statusCode, string message, TimeSpan elapsed)
    {
        LogState = logState;
        LogInput = logInput;
        StatusCode = statusCode;
        Message = message;
        Elapsed = elapsed;
    }

    public ELogState LogState { get; }
    public LogInput LogInput { get; }
    public HttpStatusCode? StatusCode { get; }
    public string Message { get; }
    public TimeSpan Elapsed { get; }
    public string Description => $"{LogState} took {Elapsed.TotalMilliseconds:0}ms. {Message} ({(StatusCode == null ? null : $"StatusCode: '{StatusCode}'")})";
}

public enum ELogState
{
    Debug,
    CallFailed,
    Exception,
    Complete
}