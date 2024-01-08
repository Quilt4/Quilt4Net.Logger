using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quilt4Net.Entities;
using Quilt4Net.Internals;

namespace Quilt4Net.Ioc;

public class InstanceContainer : IIocProxy
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly Lazy<IConfigurationDataLoader> _configurationDataLoader;
    private readonly Lazy<IMessageQueue> _messageQueue;
    private readonly Lazy<ISenderEngine> _senderEngine;
    private readonly Lazy<IHostedService> _configurationEngine;
    private readonly Lazy<ILoggerProvider> _quilt4NetProvider;

    public InstanceContainer(IConfiguration configuration, IHostEnvironment hostEnvironment, Action<Quilt4NetOptions> options)
    {
        _configuration = configuration;
        _hostEnvironment = hostEnvironment;
        _configurationDataLoader = new Lazy<IConfigurationDataLoader>(() => new ConfigurationDataLoader());
        _messageQueue = new Lazy<IMessageQueue>(() => new MessageQueue(_configurationDataLoader.Value));
        _senderEngine = new Lazy<ISenderEngine>(() => new SenderEngine(_configurationDataLoader.Value, _messageQueue.Value));
        _configurationEngine = new Lazy<IHostedService>(() => new ConfigurationEngine(_configurationDataLoader.Value, _senderEngine.Value, _messageQueue.Value));
        _quilt4NetProvider = new Lazy<ILoggerProvider>(() => new Quilt4NetProvider(this, options));
    }

    public T GetService<T>()
    {
        switch (typeof(T).Name)
        {
            case nameof(IConfiguration):
                return (T)_configuration;
            case nameof(IHostEnvironment):
                return (T)_hostEnvironment;
            case nameof(ILoggerProvider):
                return (T)_quilt4NetProvider.Value;
            case nameof(IConfigurationDataLoader):
                return (T)_configurationDataLoader.Value;
            case nameof(ConfigurationEngine):
                return (T)_configurationEngine.Value;
            case nameof(ISenderEngine):
                return (T)_senderEngine.Value;
            case nameof(IMessageQueue):
                return (T)_messageQueue.Value;
            default:
                Debugger.Break();
                throw new ArgumentOutOfRangeException($"Cannot find instance of type '{typeof(T).Name}'.");
        }
    }
}