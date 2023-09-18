﻿using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quilt4Net.Internals;

namespace Quilt4Net;

[ProviderAlias("Quilt4NetLogger")]
internal class Quilt4NetProvider : ILoggerProvider
{
    protected readonly IServiceProvider _serviceProvider;

    public Quilt4NetProvider(IServiceProvider serviceProvider, Action<Quilt4NetOptions> options)
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
        var sender = _serviceProvider.GetService<ISender>();
        var configurationLoader = _serviceProvider.GetService<IConfigurationDataLoader>();
        return new Quilt4NetLogger(sender, configurationLoader, categoryName);
    }

    public void Dispose()
    {
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

    private LogDataItem ToLogData(KeyValuePair<string, object> x)
    {
        return new LogDataItem
        {
            Key = x.Key,
            Value = JsonSerializer.Serialize(x.Value),
            Type = x.Value.GetType().FullName
        };
    }

    protected virtual (string EnvironmentName, string ApplicationName) GetAppName()
    {
        var hostEnvironment = _serviceProvider.GetService<IHostEnvironment>();
        return (hostEnvironment?.EnvironmentName, hostEnvironment?.ApplicationName);
    }
}