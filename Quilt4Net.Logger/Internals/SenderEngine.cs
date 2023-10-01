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
    private string _appDataKey;
    private Configuration _configuration = new();
    private bool _isConfigured;

    public SenderEngine(IConfigurationDataLoader configurationDataLoader, IMessageQueue messageQueue)
    {
        _messageQueue = messageQueue;
        _configurationData = configurationDataLoader.Get();
        _httpClient = CreateHttpClient(_configurationData.BaseAddress);
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                if (!_isConfigured)
                {
                    _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Debug, null, null, "Waiting for Quilt4Net configuration."));
                    await Task.Delay(TimeSpan.FromSeconds(5), _cancellationTokenSource.Token);
                    continue;
                }

                try
                {
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
        return Task.CompletedTask;
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
        var httpClient = new HttpClient();
        httpClient.BaseAddress = address;
        return httpClient;
    }

    private async Task SendAsync(LogInput logInput)
    {
        if (_configuration?.LogLevel != null && logInput.LogLevel < _configuration.LogLevel)
        {
            _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, $"Skip because only logging {_configuration.LogLevel} and above, this message was {logInput.LogLevel}.", null));
            return;
        }

        var sw = new Stopwatch();
        sw.Start();
        //_logEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, "Initiate send.", sw.Elapsed));

        try
        {
            if (string.IsNullOrEmpty(_appDataKey))
            {
                using var appDataContent = JsonContent.Create(logInput.AppData);
                appDataContent.Headers.Add("X-API-KEY", _configurationData.ApiKey);
                using var appDataResponse = await _httpClient.PostAsync("Collect/application", appDataContent);
                if (!appDataResponse.IsSuccessStatusCode)
                {
                    await ShowFailMessage(logInput, appDataResponse, sw);
                }
                else
                {
                    _appDataKey = await appDataResponse.Content.ReadAsStringAsync();
                }
            }

            logInput = logInput with
            {
                AppDataKey = _appDataKey,
                AppData = null
            };

            using var content = JsonContent.Create(logInput);
            content.Headers.Add("X-API-KEY", _configurationData.ApiKey);

            //_logEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, "Post starting.", sw.Elapsed));
            //using var response = await _httpClient.PostAsync("Collect/inject", content); //NOTE: Inject directly
            using var response = await _httpClient.PostAsync("Collect", content); //NOTE: Add to inbox on server
            //_logEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, "Post complete.", sw.Elapsed));

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    _messageQueue.Enqueue(logInput);
                    _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Warning, logInput, response.StatusCode, $"{response.ReasonPhrase} Waiting and retrying.", sw.StopAndGetElapsed()));
                    await Task.Delay(TimeSpan.FromSeconds(2));
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
            var limit = TimeSpan.FromMilliseconds(_configuration?.SendIntervalLimitMilliseconds ?? 500);
            var wait = limit - sw.Elapsed;
            if (wait.Ticks > 0)
            {
                _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Debug, null, null, $"Wait for {wait.TotalMilliseconds:0}ms."));
                await Task.Delay(wait);
            }
        }
        catch (Exception e)
        {
            //TODO: Consider requeue
            _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Exception, logInput, null, e.Message, sw.StopAndGetElapsed()));
        }
    }

    private async Task ShowFailMessage(LogInput logInput, HttpResponseMessage response, Stopwatch sw)
    {
        var payload = await response.Content.ReadAsStringAsync();
        var errorMessage = GetErrorMessage(payload);
        var message = errorMessage?.Message ?? payload;
        if (string.IsNullOrEmpty(message)) message = response.ReasonPhrase;
        _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.CallFailed, logInput, response.StatusCode, message, sw.StopAndGetElapsed()));
    }

    public async Task<Configuration> GetConfigurationAsync(CancellationToken cancellationToken)
    {
        using var content = new HttpRequestMessage(HttpMethod.Get, $"Collect?MinLogLevel={(int)_configurationData.MinLogLevel}");
        content.Headers.Add("X-API-KEY", _configurationData.ApiKey);

        using var result = await _httpClient.SendAsync(content, cancellationToken);
        if (result.IsSuccessStatusCode)
        {
            try
            {
                _configuration = await result.Content.ReadFromJsonAsync<Configuration>(cancellationToken: cancellationToken);
                _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Debug, null, result.StatusCode, $"Log level set to {(LogLevel)_configuration.LogLevel} and rate limit to {_configuration.SendIntervalLimitMilliseconds}ms on channel '{_configuration.Name}'."));
                _isConfigured = true;
                return _configuration;
            }
            catch (Exception e)
            {
                Debugger.Break();
                throw;
            }
        }

        _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.CallFailed, null, result.StatusCode, $"Got '{result.ReasonPhrase}' when calling configuration.", null));
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