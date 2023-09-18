namespace Quilt4Net.Internals;

internal class LogScope<TState> : IDisposable
{
    private readonly TState _state;

    public LogScope(TState state)
    {
        _state = state;
    }

    public void Dispose()
    {
    }
}