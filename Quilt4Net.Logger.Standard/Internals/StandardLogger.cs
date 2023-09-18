using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;

namespace Quilt4Net.Internals
{
    internal class Quilt4NetStandardLogger : ILogger
    {
        private static ISender _sender;
        private readonly string _categoryName;
        private readonly LogLevel _minLogLevel;
        private readonly LogAppData _appData;

        internal Quilt4NetStandardLogger(ISender sender, IConfigurationDataLoader configurationDataLoader, string categoryName = null)
        {
            _sender = sender;
            _categoryName = categoryName;
            var configuration = configurationDataLoader.Get();
            _minLogLevel = configuration.MinLogLevel;
            _appData = configuration.AppData;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var logData = ((IEnumerable<KeyValuePair<string, object>>)state).ToLogData();

            var parsedMessage = formatter.Invoke(state, exception);
            logData.AddField("message", parsedMessage);

            var logEntry = new LogMessage
            {
                Message = parsedMessage,
                Exception = exception,
                LogLevel = logLevel,
                Data = logData
            };

            Send(logEntry);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _minLogLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new LogScope<TState>(state);
        }

        private void Send(LogMessage logMessage)
        {
            var logInput = new LogInput
            {
                CategoryName = _categoryName,
                LogLevel = (int)logMessage.LogLevel,
                Message = logMessage.Message,
                AppData = _appData,
                Data = logMessage.GetData()
                    .Where(x => x.Key != "Message" && $"{x.Value}" != logMessage.Message)
                    .Select(ToLogDataItem).ToArray(),
            };

            _sender.Send(logInput);
        }

        protected virtual LogDataItem ToLogDataItem(KeyValuePair<string, object> x)
        {
            //var type = x.Value.GetType();
            //var serializer = new XmlSerializer(type);
            //string value;
            //using (var writer = new StringWriter())
            //{
            //    serializer.Serialize(writer, x.Value);
            //    value = writer.ToString();
            //}

            //return new LogDataItem
            //{
            //    Key = x.Key,
            //    Value = value,
            //    Type = x.Value.GetType().FullName
            //};
            throw new NotImplementedException();
        }
    }
}