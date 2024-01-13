using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quilt4Net.Dtos;
using Quilt4Net.Entities;
using Quilt4Net.Internals;

namespace Quilt4Net.Ioc;

public class InstanceContainer : IIocProxy
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly Lazy<IConfigurationData> _configurationData;
    private readonly Lazy<IMessageQueue> _messageQueue;
    private readonly Lazy<ISenderEngine> _senderEngine;
    private readonly Lazy<IConfigurationEngine> _configurationEngine;
    private readonly Lazy<ILoggerProvider> _quilt4NetProvider;

    public InstanceContainer(IConfiguration configuration, IHostEnvironment hostEnvironment, Action<Quilt4NetOptions> options)
    {
        _configuration = configuration;
        _hostEnvironment = hostEnvironment;
        _quilt4NetProvider = new Lazy<ILoggerProvider>(() => new Quilt4NetProvider(this, options));
        _configurationData = new Lazy<IConfigurationData>(() => ((Quilt4NetProvider)_quilt4NetProvider.Value).ConfigurationData);
        _messageQueue = new Lazy<IMessageQueue>(() => new MessageQueue(_configurationData.Value));
        _senderEngine = new Lazy<ISenderEngine>(() => new SenderEngine(_configurationData.Value, _messageQueue.Value));
        _configurationEngine = new Lazy<IConfigurationEngine>(() => new ConfigurationEngine(_configurationData.Value, _senderEngine.Value, _messageQueue.Value));
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
            case nameof(IConfigurationData):
                return (T)_configurationData.Value;
            case nameof(IConfigurationEngine):
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