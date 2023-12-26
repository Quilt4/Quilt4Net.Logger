using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Quilt4Net.Internals;

internal class SenderEngine : ISenderEngine
{
    private readonly IMessageQueue _messageQueue;
    private readonly ConfigurationData _configurationData;
    private readonly HttpClient _httpClient;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private Configuration _configuration = new();
    private bool _isConfigured;
    private readonly Stopwatch _sw;
    private readonly SemaphoreSlim _lock = new (1, 1);
    private bool _started;
    private int _lastQueueCountSent;
    private string _appDataKey;
    private string _sessionDataKey;

    public SenderEngine(IConfigurationDataLoader configurationDataLoader, IMessageQueue messageQueue)
    {
        _messageQueue = messageQueue;
        _configurationData = configurationDataLoader.Get();
        _httpClient = CreateHttpClient(_configurationData.BaseAddress);
        _cancellationTokenSource = new CancellationTokenSource();
        _messageQueue.QueueEvent += async (_, e) =>
        {
            if (_lastQueueCountSent == e.QueueCount) return;
            if (!_isConfigured) return;

            using var content = JsonContent.Create(new QueueState { Count = e.QueueCount });
            using var response = await _httpClient.PostAsync("Collect/queue", content);
            if (!response.IsSuccessStatusCode)
            {
                await ShowFailMessage(null, response, null);
            }
            _lastQueueCountSent = e.QueueCount;
        };

    _sw = new Stopwatch();
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
                    if (!_isConfigured)
                    {
                        _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Debug, null, null, "Waiting for Quilt4Net configuration."));
                        await Task.Delay(TimeSpan.FromSeconds(5), _cancellationTokenSource.Token);
                        continue;
                    }

                    var item = _messageQueue.DequeueOne(_cancellationTokenSource.Token);
                    await SendAsync(item);
                }
                catch (Exception e)
                {
                    _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Exception, null, null, e.Message));
                    await Task.Delay(TimeSpan.FromSeconds(5), _cancellationTokenSource.Token);
                }
            }
        }, _cancellationTokenSource.Token);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }

    private HttpClient CreateHttpClient(string baseAddress)
    {
        if (!baseAddress.EndsWith("/")) baseAddress += "/";
        if (!Uri.TryCreate(baseAddress, UriKind.Absolute, out var address)) throw new InvalidOperationException($"Cannot parse '{baseAddress}' to an absolute uri.");
        var httpClient = _configurationData.HttpClientFactory?.Invoke() ?? new HttpClient();
        httpClient.DefaultRequestHeaders.Add("mode", "no-cors");
        httpClient.DefaultRequestHeaders.Add("X-API-KEY", _configurationData.ApiKey);
        httpClient.BaseAddress = address;
        return httpClient;
    }

    private async Task SendAsync(LogInput logInput)
    {
        if (_configuration?.Filter?.LogLevel != null && logInput.LogLevel < _configuration?.Filter?.LogLevel)
        {
            _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, $"Skip because only logging {_configuration?.Filter?.LogLevel} and above, this message was {logInput.LogLevel}."));
            return;
        }

        var sw = new Stopwatch();
        sw.Start();

        try
        {
            logInput = logInput with
            {
                AppDataKey = _appDataKey,
                SessionDataKey = _sessionDataKey
            };

            using var content = JsonContent.Create(logInput);
            _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Timer, logInput, null, $"{_sw.GetElapsedAndReset().TotalMilliseconds:0} since last send. (LogInput)"));
            using var response = await _httpClient.PostAsync("Collect", content); //NOTE: Add to inbox on server

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    _messageQueue.Enqueue(logInput);
                    _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Warning, logInput, response.StatusCode, $"{response.ReasonPhrase} Waiting and retrying.", sw.StopAndGetElapsed()));
                    await GetConfigurationAsync(CancellationToken.None);
                    await Task.Delay(TimeSpan.FromMilliseconds(1000));
                }
                else
                {
                    await ShowFailMessage(logInput, response, sw);
                }
            }
            else
            {
                string resourceLocation = null;
                if (response.Headers.TryGetValues("Location", out var locationValues)) resourceLocation = locationValues.FirstOrDefault();
                _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Complete, logInput, response.StatusCode, resourceLocation, sw.StopAndGetElapsed()));
            }

            //NOTE: Limit the send rate to server
            await Wait(sw);
        }
        catch (Exception e)
        {
            Debugger.Break(); //Consider requeue
            _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Exception, logInput, null, e.Message, sw.StopAndGetElapsed()));
        }
    }

    private async Task Wait(Stopwatch sw)
    {
        var limit = TimeSpan.FromMilliseconds((_configuration?.SendIntervalLimitMilliseconds ?? 500));
        var wait = limit - sw.Elapsed;
        if (wait.Ticks > 0)
        {
            _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Debug, null, null, $"Wait for {wait.TotalMilliseconds:0}ms."));
            await Task.Delay(wait);
            sw.Stop();
            sw.Reset();
            sw.Start();
        }
    }

    //private async Task<string> SendAppData(LogInput logInput, Stopwatch sw)
    //{
    //    //if (string.IsNullOrEmpty(_appDataKey))
    //    //{
    //    //    using var appDataContent = JsonContent.Create(logInput.AppData);
    //    //    //appDataContent.Headers.Add("X-API-KEY", _configurationData.ApiKey);
    //    //    var message = $"{_sw.GetElapsedAndReset().TotalMilliseconds:0} since last send. (AppData)";
    //    //    _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Timer, logInput, null, message));
    //    //    using var appDataResponse = await _httpClient.PostAsync("Collect/application", appDataContent);
    //    //    if (!appDataResponse.IsSuccessStatusCode)
    //    //    {
    //    //        if (appDataResponse.StatusCode == HttpStatusCode.TooManyRequests)
    //    //        {
    //    //            _messageQueue.Enqueue(logInput);
    //    //            _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Warning, logInput, appDataResponse.StatusCode, $"{appDataResponse.ReasonPhrase} Waiting and retrying.", sw.StopAndGetElapsed()));
    //    //            await GetConfigurationAsync(CancellationToken.None);
    //    //            await Task.Delay(TimeSpan.FromMilliseconds(1000));
    //    //        }
    //    //        else
    //    //        {
    //    //            await ShowFailMessage(logInput, appDataResponse, sw);
    //    //        }
    //    //    }
    //    //    else
    //    //    {
    //    //        _appDataKey = await appDataResponse.Content.ReadAsStringAsync();
    //    //    }

    //    //    await Wait(sw);
    //    //}

    //    //return _appDataKey;
    //    throw new NotImplementedException();
    //}

    private async Task ShowFailMessage(LogInput logInput, HttpResponseMessage response, Stopwatch sw)
    {
        var payload = await response.Content.ReadAsStringAsync();
        var errorMessage = GetErrorMessage(payload);
        var message = errorMessage?.Message ?? payload;
        if (string.IsNullOrEmpty(message)) message = response.ReasonPhrase;
        _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.CallFailed, logInput, response.StatusCode, message, sw?.StopAndGetElapsed()));
    }

    public async Task<Configuration> GetConfigurationAsync(CancellationToken cancellationToken)
    {
        using var content = JsonContent.Create(new StartupRequest
        {
            AppData = _configurationData.AppData,
            SessionData = _configurationData.SessionData
        });

        using var result = await _httpClient.PostAsync($"Collect/start/{(int)_configurationData.MinLogLevel}", content, cancellationToken);
        if (result.IsSuccessStatusCode)
        {
            try
            {
                var responseString = await result.Content.ReadAsStringAsync(cancellationToken);
                if (string.IsNullOrEmpty(responseString)) return null;
                var response = JsonSerializer.Deserialize<StartupResponse>(responseString);
                _configuration = response.Configuration;
                _appDataKey = response.AppDataKey;
                _sessionDataKey = response.SessionDataKey;
                if (_configuration == null) return null;
                _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Debug, null, result.StatusCode, $"Log level set to {(LogLevel?)_configuration.Filter?.LogLevel} and rate limit to {_configuration?.SendIntervalLimitMilliseconds}ms on channel '{_configuration?.Name}'."));
                _isConfigured = true;
                return _configuration;
            }
            catch (Exception e)
            {
                Debugger.Break();
                _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Exception, null, null, e.Message));
                throw;
            }
        }

        _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.CallFailed, null, result.StatusCode, $"Got '{result.ReasonPhrase}' when calling configuration."));
        return null;
    }

    protected virtual ErrorMessage GetErrorMessage(string payload)
    {
        if (!payload.StartsWith("{")) return null;

        try
        {
            var errorMessage = JsonSerializer.Deserialize<ErrorMessage>(payload, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return errorMessage;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}