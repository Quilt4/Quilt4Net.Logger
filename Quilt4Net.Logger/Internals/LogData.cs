using System.Collections;

namespace Quilt4Net.Internals;

internal class LogData : IEnumerable<KeyValuePair<string, object>>
{
    private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static LogData Build(params KeyValuePair<string, object>[] items)
    {
        return new LogData();
    }

    internal void AddData<T>(string key, T data)
    {
        _data.TryAdd(key, data);
    }
}