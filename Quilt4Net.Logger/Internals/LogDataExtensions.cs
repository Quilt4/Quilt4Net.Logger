using System.Collections;
using System.Diagnostics;
using System.Text.Json;
using Quilt4Net.Dtos;

namespace Quilt4Net.Internals;

internal static class LogDataExtensions
{
    public static LogDataItem ToLogDataItem(this KeyValuePair<string, object> x)
    {
        TrySerializeValue(x.Value, out var serialize);

        return new LogDataItem
        {
            Key = x.Key,
            Value = serialize,
            Type = x.Value?.GetType().FullName
        };
    }

    public static LogData ToLogData(this IEnumerable<KeyValuePair<string, object>> item)
    {
        var logData = new LogData();
        foreach (var data in item)
        {
            logData.AddField(data.Key, data.Value);
        }

        return logData;
    }

    public static TLogData AddField<TLogData, T>(this TLogData logData, string key, T data)
        where TLogData : LogData
    {
        logData.AddData(key, data);
        return logData;
    }

    public static Dictionary<string, object> GetData(this LogData logData)
    {
        var data = logData?.ToDictionary(x => x.Key, x => x.Value) ?? new Dictionary<string, object>();
        return data;
    }

    public static TLogData AddFields<TLogData>(this TLogData logData, LogData data)
        where TLogData : LogData
    {
        if (data == null) return logData;

        foreach (var item in data)
        {
            logData.AddData(item.Key, item.Value);
        }

        return logData;
    }

    public static Dictionary<string, object> GetData(this LogMessage logMessage)
    {
        var data = new Dictionary<string, object>();
        if (logMessage.Data != null)
        {
            foreach (var item in logMessage.Data)
            {
                data.TryAdd(item.Key, item.Value);
                //var data = logMessage.Data?.ToDictionary(x => x.Key, x => x.Value) ?? new Dictionary<string, object>();
            }
        }

        if (logMessage.Exception != null)
        {
            data.TryAdd(Constants.StackTrace, logMessage.Exception.StackTrace);

            foreach (DictionaryEntry entry in logMessage.Exception.Data)
            {
                data.TryAdd(entry.Key.ToString(), entry.Value);
            }
        }

        data.TryAdd(Constants.OriginalFormat, logMessage.Message);

        return data;
    }

    private static bool TrySerializeValue(object val, out string data)
    {
        data = null;

        if (val == null) return false;

        try
        {
            data = JsonSerializer.Serialize(val);
            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Cannot Serialize value of type '{val.GetType().FullName}'.");
            Debugger.Break();
            return false;
        }
    }
}