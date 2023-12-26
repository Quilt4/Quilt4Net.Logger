using Microsoft.Extensions.Hosting;

namespace Quilt4Net.Internals;

internal class ConfigurationEngine : IHostedService
{
    private readonly ISenderEngine _sender;
    private readonly IMessageQueue _messageQueue;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private bool _started;
    private readonly ConfigurationData _configurationData;

    public ConfigurationEngine(IConfigurationDataLoader configurationDataLoader, ISenderEngine sender, IMessageQueue messageQueue)
    {
        _configurationData = configurationDataLoader.Get();
        _sender = sender;
        _messageQueue = messageQueue;
        _cancellationTokenSource = new CancellationTokenSource();
    }

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
                        _messageQueue.SetConfiguration(configuration);
                    }

                    await Task.Delay(TimeSpan.FromMinutes(5), _cancellationTokenSource.Token);
                }
                catch (Exception e)
                {
                    _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Exception, null, null, e.Message));
                    await Task.Delay(TimeSpan.FromMinutes(5), _cancellationTokenSource.Token);
                }
            }
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}