using System.Collections.Generic;
using Quilt4Net.Internals.Standard;

namespace Quilt4Net
{
    internal class LoggingDefaultData : ILoggingDefaultData
    {
        private readonly IDictionary<string, object> _data = new Dictionary<string, object>();

        public ILoggingDefaultData AddData(string key, object value)
        {
            _data.TryAdd(key, value);
            return this;
        }

        public IDictionary<string, object> GetData()
        {
            return _data;
        }
    }
}