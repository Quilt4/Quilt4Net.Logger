using Microsoft.Extensions.Hosting;

namespace Quilt4Net.Internals;

internal class ConfigurationEngine : IHostedService
{
    private readonly ISender _sender;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private bool _started;

    public ConfigurationEngine(ISender sender)
    {
        _sender = sender;
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
                //TODO: Load config here
                await _sender.GetConfigurationAsync(_cancellationTokenSource.Token);

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

internal interface ISender : IDisposable
{
    void Send(LogInput logInput);
    Task GetConfigurationAsync(CancellationToken cancellationToken);
}