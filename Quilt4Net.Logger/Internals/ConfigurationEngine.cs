using System.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace Quilt4Net.Internals;

internal class ConfigurationEngine : IHostedService
{
    private readonly ISenderEngine _sender;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private bool _started;

    public ConfigurationEngine(ISenderEngine sender)
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
                try
                {
                    await _sender.UpdateConfigurationAsync(_cancellationTokenSource.Token);
                    await Task.Delay(TimeSpan.FromMinutes(5), _cancellationTokenSource.Token);
                }
                catch (Exception e)
                {
                    Debugger.Break();
                    Console.WriteLine(e);
                    throw;
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