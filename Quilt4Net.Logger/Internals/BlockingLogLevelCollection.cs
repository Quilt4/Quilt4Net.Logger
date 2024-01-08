using System.Collections.Concurrent;
using Quilt4Net.Dtos;

namespace Quilt4Net.Internals;

internal class BlockingLogLevelCollection : BlockingCollection<LogInput>
{
    private readonly LogLevelQueue _logLevelQueue;

    public BlockingLogLevelCollection(LogLevelQueue logLevelQueue)
        : base(logLevelQueue)
    {
        _logLevelQueue = logLevelQueue;
    }

    public new bool TryAdd(LogInput item)
    {
        try
        {
            return base.TryAdd(item);
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    public new void Add(LogInput item)
    {
        try
        {
            base.Add(item);
        }
        catch (InvalidOperationException)
        {
        }
    }
}