﻿using System.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace Quilt4Net.Internals;

internal class ConfigurationEngine : IHostedService
{
    private readonly ISenderEngine _sender;
    private readonly IMessageQueue _messageQueue;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private bool _started;

    public ConfigurationEngine(ISenderEngine sender, IMessageQueue messageQueue)
    {
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
            await Task.Delay(TimeSpan.FromSeconds(5), _cancellationTokenSource.Token);

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    var configuration = await _sender.GetConfigurationAsync(_cancellationTokenSource.Token);
                    if (configuration != null)
                    {
                        _messageQueue.SetConfiguration(configuration);
                    }
                }
                catch (Exception e)
                {
                    Debugger.Break();
                    Console.WriteLine(e.Message);
                    Trace.TraceError(e.Message);
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