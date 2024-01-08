using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using Quilt4Net.Dtos;

namespace Quilt4Net.Internals;

internal class LogLevelQueue : IProducerConsumerCollection<LogInput>
{
    private readonly int _itemLimit;
    private readonly ConcurrentDictionary<int, ConcurrentQueue<LogInput>> _queues = new();
    private readonly SemaphoreSlim _lock = new(1, 1);
    private int _count;

    public LogLevelQueue(int itemLimit)
    {
        _itemLimit = itemLimit;
    }

    public IEnumerator<LogInput> GetEnumerator()
    {
        return _queues.SelectMany(x => x.Value).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void CopyTo(Array array, int index)
    {
        throw new NotImplementedException();
    }

    public int Count => _count; //_queues.Sum(x => x.Value.Count);
    public bool IsSynchronized => throw new NotImplementedException();
    public object SyncRoot => throw new NotImplementedException();
    public void CopyTo(LogInput[] array, int index)
    {
        throw new NotImplementedException();
    }

    public LogInput[] ToArray()
    {
        return _queues.SelectMany(x => x.Value).ToArray();
    }

    public bool TryAdd(LogInput item)
    {
        bool added = true;

        try
        {
            _lock.Wait();

            if (Count >= _itemLimit)
            {
                if (!TryTakeReversed(out var trash))
                {
                    throw new InvalidOperationException("Cannot remove anything from the queue.");
                }

                added = false;
            }

            if (!_queues.TryGetValue(item.LogLevel, out var queue))
            {
                queue = new ConcurrentQueue<LogInput>();
                if (!_queues.TryAdd(item.LogLevel, queue))
                {
                    if (!_queues.TryGetValue(item.LogLevel, out queue))
                    {
                        Debugger.Break();
                        throw new InvalidOperationException($"Cannot get or add queue with level {item.LogLevel}.");
                    }
                }
            }

            queue.Enqueue(item);
            Interlocked.Increment(ref _count);
            return added;

        }
        finally
        {
            _lock.Release();
        }
    }

    public bool TryTake(out LogInput item)
    {
        try
        {
            _lock.Wait();

            foreach (var queue in _queues.OrderByDescending(x => x.Key))
            {
                var success = queue.Value.TryDequeue(out item);
                if (success)
                {
                    Interlocked.Decrement(ref _count);
                    return true;
                }
            }

            item = default;
            return false;
        }
        finally
        {
            _lock.Release();
        }
    }

    private bool TryTakeReversed(out LogInput item)
    {
        foreach (var queue in _queues.OrderBy(x => x.Key))
        {
            var success = queue.Value.TryDequeue(out item);
            if (success)
            {
                Interlocked.Decrement(ref _count);
                return true;
            }
        }

        item = default;
        return false;
    }
}