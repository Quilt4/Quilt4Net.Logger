using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Quilt4Net.Internals.Standard;

namespace Quilt4Net.Internals
{
    internal static class LogDataExtensions
    {
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
            var data = logMessage.Data?.ToDictionary(x => x.Key, x => x.Value) ?? new Dictionary<string, object>();

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
    }
}