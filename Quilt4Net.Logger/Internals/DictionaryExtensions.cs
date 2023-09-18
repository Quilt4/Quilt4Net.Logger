namespace Quilt4Net.Internals;

internal static class DictionaryExtensions
{
    public static Dictionary<string, object> TryAddRange(this Dictionary<string, object> first, Dictionary<string, object> second)
    {
        var result = new Dictionary<string, object>();

        foreach (var item in first)
        {
            result.TryAdd(item.Key, item.Value);
        }

        foreach (var item in second)
        {
            result.TryAdd(item.Key, item.Value);
        }

        return result;
    }
}