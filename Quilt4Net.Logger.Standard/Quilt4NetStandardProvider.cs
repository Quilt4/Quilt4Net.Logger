using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Configuration;
using Quilt4Net.Internals;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Quilt4Net
{
    [ProviderAlias("Quilt4NetStandardLogger")]
    public class Quilt4NetStandardProvider : ILoggerProvider
    {
        protected readonly IServiceProvider _serviceProvider;

        public Quilt4NetStandardProvider(IServiceProvider serviceProvider, Action<Quilt4NetOptions> options = null)
        {
            _serviceProvider = serviceProvider;

            var configurationDataLoader = serviceProvider.GetService<IConfigurationDataLoader>();
            configurationDataLoader.Set(() =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var o = LoadQuilt4NetOptions(configuration, options);
                var minLogLevel = Enum.TryParse(configuration?.GetSection("Logging").GetSection("LogLevel").GetSection("Default").Value, true, out LogLevel level) ? level : LogLevel.Information;
                var appData = GetLoggerData(configuration, GetAppName(), o);

                return new ConfigurationData
                {
                    BaseAddress = o.BaseAddress,
                    ApiKey = o.ApiKey,
                    MinLogLevel = minLogLevel,
                    AppData = appData,
                    LogCompleteEvent = o.LogCompleteEvent,
                    LogFailEvent = o.LogFailEvent,
                };
            });
        }

        public virtual ILogger CreateLogger(string categoryName)
        {
            var configurationDataLoader = _serviceProvider.GetService<IConfigurationDataLoader>();

            var sender = _serviceProvider.GetService<ISender>();
            return new Quilt4NetStandardLogger(sender, configurationDataLoader, categoryName);
        }

        public void Dispose()
        {
        }

        protected virtual (string EnvironmentName, string ApplicationName) GetAppName()
        {
            return (null, null);
        }

        private Quilt4NetOptions LoadQuilt4NetOptions(IConfiguration configuration, Action<Quilt4NetOptions> options)
        {
            var o = new Quilt4NetOptions
            {
                ApiKey = configuration?["Quilt4Net:ApiKey"],
                BaseAddress = configuration?["Quilt4Net:BaseAddress"] ?? "https://quilt4net.com/api/",
            };

            options?.Invoke(o);
            return o;
        }

        private LogAppData GetLoggerData(IConfiguration configuration, (string EnvironmentName, string ApplicationName) name, Quilt4NetOptions o)
        {
            var assemblyName = Assembly.GetEntryAssembly()?.GetName();

            var appData = new LogAppData
            {
                Environment = name.EnvironmentName ?? configuration["Environment"],
                Application = name.ApplicationName ?? assemblyName?.Name,
                Version = assemblyName?.Version?.ToString(),
                Machine = Environment.MachineName,
                SystemUser = Environment.UserName,
                Data = o.LoggingDefaultData.GetData().Select(ToLogData).ToArray(),
            };

            return appData;
        }

        protected virtual LogDataItem ToLogData(KeyValuePair<string, object> x)
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
            //    Type = type.FullName
            //};
            throw new NotImplementedException();
        }
    }
}