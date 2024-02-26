using Quilt4Net.Dtos;
using Quilt4Net.Entities;

namespace Quilt4Net.Internals;

internal class ConfigurationEngine : IConfigurationEngine
{
    private static readonly SemaphoreSlim _lock = new(1, 1);
    private readonly ISenderEngine _sender;
    private readonly IMessageQueue _messageQueue;
    private readonly ConfigurationData _configurationData;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private bool _started;
    private bool _haveConfiguration;

    public ConfigurationEngine(IConfigurationData configurationData, ISenderEngine sender, IMessageQueue messageQueue)
    {
        _configurationData = (ConfigurationData)configurationData;
        _sender = sender;
        _messageQueue = messageQueue;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public bool HaveConfiguration => _haveConfiguration;

    public event EventHandler<ConfigurationEvent> ConfigurationEvent;
    public bool Started => _started;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _lock.WaitAsync(cancellationToken);
            if (_started) return;
            StartEngine();
            _started = true;
        }
        finally
        {
            _lock.Release();
        }
    }

    private void StartEngine()
    {
        Task.Run(async () =>
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    var configuration = await _sender.GetConfigurationAsync(_cancellationTokenSource.Token);
                    if (configuration != null)
                    {
                        _haveConfiguration = true;
                        ConfigurationEvent?.Invoke(this, new ConfigurationEvent(EConfigurationAction.Success));
                        _messageQueue.SetConfiguration(configuration);
                    }

                }
                catch (Exception e)
                {
                    _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Exception, null, null, e.Message));
                    ConfigurationEvent?.Invoke(this, new ConfigurationEvent(EConfigurationAction.Crash, e));
                }

                await Task.Delay(TimeSpan.FromMinutes(5), _cancellationTokenSource.Token);
            }
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}