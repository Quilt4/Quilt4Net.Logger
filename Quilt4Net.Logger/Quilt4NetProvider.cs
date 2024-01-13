using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quilt4Net.Dtos;
using Quilt4Net.Entities;
using Quilt4Net.Internals;
using Quilt4Net.Ioc;

namespace Quilt4Net;

[ProviderAlias("Quilt4NetLogger")]
public class Quilt4NetProvider : ILoggerProvider
{
    protected readonly IIocProxy _iocProxy;
    private readonly Action<Quilt4NetOptions> _optionsLoader;
    private IConfigurationData _configurationData;
    private IStateService _stateService;
    private Quilt4NetOptions _options;

    public Quilt4NetProvider(IServiceProvider serviceProvider, Action<Quilt4NetOptions> options)
        : this(new ServiceProviderIocProxy(serviceProvider), options)
    {
    }

    public Quilt4NetProvider(IIocProxy iocProxy, Action<Quilt4NetOptions> options)
    {
        _iocProxy = iocProxy;
        _optionsLoader = options;

        var configurationDataLoader = _iocProxy.GetService<IConfigurationDataLoader>();
        configurationDataLoader.Set(() => (ConfigurationData)ConfigurationData);
    }

    internal Quilt4NetOptions Options
    {
        get
        {
            if (_options != null) return _options;

            var configuration = _iocProxy.GetService<IConfiguration>();
            _options ??= LoadQuilt4NetOptions(configuration, _optionsLoader);
            return _options;
        }
    }

    internal IConfigurationData ConfigurationData
    {
        get
        {
            if (_configurationData != null) return _configurationData;

            var configuration = _iocProxy.GetService<IConfiguration>();
            var minLogLevel = Enum.TryParse(configuration?.GetSection("Logging").GetSection("LogLevel").GetSection("Default").Value, true, out LogLevel level) ? level : LogLevel.Information;
            var appName = GetAppName();
            var appData = GetAppData(configuration, appName.ApplicationName, Options);
            var sessionData = GetSessionData(configuration, appName.EnvironmentName, Options);
            _configurationData = new ConfigurationData
            {
                BaseAddress = Options.BaseAddress,
                ApiKey = Options.ApiKey,
                MinLogLevel = minLogLevel,
                AppData = appData,
                SessionData = sessionData,
                LogEvent = Options.LogEvent,
                HttpClientFactory = Options.HttpClientFactory,
            };
            return _configurationData;
        }
    }

    public virtual ILogger CreateLogger(string categoryName)
    {
        _stateService ??= _iocProxy.GetService<IStateService>();
        var messageQueue = _iocProxy.GetService<IMessageQueue>();
        var configurationLoader = _iocProxy.GetService<IConfigurationDataLoader>();
        var configuration = _iocProxy.GetService<IConfigurationData>();
        return new Quilt4NetLogger(messageQueue, configurationLoader, configuration, categoryName);
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

    private LogAppData GetAppData(IConfiguration configuration, string applicationName, Quilt4NetOptions o)
    {
        var assemblyName = Assembly.GetEntryAssembly()?.GetName();
        var loggerInfo = Assembly.GetExecutingAssembly().GetName();

        var data = new LogAppData
        {
            Application = configuration?["Application"].NullIfEmpty() ?? applicationName.NullIfEmpty() ?? assemblyName?.Name.NullIfEmpty() ?? "Unknown",
            Version = assemblyName?.Version?.ToString(),
            LoggerInfo = $"{loggerInfo.Name} {loggerInfo.Version}"
        };

        return data;
    }

    private LogSessionData GetSessionData(IConfiguration configuration, string environmentName, Quilt4NetOptions o)
    {
        var data = new LogSessionData
        {
            Environment = o.Environment.NullIfEmpty() ?? configuration?["Environment"].NullIfEmpty() ?? environmentName.NullIfEmpty() ?? "Unknown",
            Machine = Environment.MachineName,
            SystemUser = Environment.UserName,
            Data = o.LoggingDefaultData.GetData().Select(ToLogData).ToArray(),
            ClientTime = DateTimeOffset.Now,
            CurrentUser = null,
        };

        return data;
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
        var hostEnvironment = _iocProxy.GetService<IHostEnvironment>();
        return (hostEnvironment?.EnvironmentName, hostEnvironment?.ApplicationName);
    }
}