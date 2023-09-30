using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace Quilt4Net.Internals;

internal class SenderEngine : ISenderEngine
{
    private readonly IMessageQueue _messageQueue;
    private readonly ConfigurationData _configurationData;
    private readonly HttpClient _httpClient;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private string _appDataKey;
    private Configuration _configuration = new();

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
                try
                {
                    var item = _messageQueue.DequeueOne(_cancellationTokenSource.Token);
                    //_configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Debug, null, null, $"There are {_messageQueue.QueueCount} items in queue.", TimeSpan.Zero));
                    await SendAsync(item);
                }
                catch (Exception e)
                {
                    Debugger.Break();
                    Console.WriteLine(e);
                    throw;
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
        var sw = new Stopwatch();
        sw.Start();
        //_logEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, "Initiate send.", sw.Elapsed));

        if (string.IsNullOrEmpty(_appDataKey))
        {
            using var content = JsonContent.Create(logInput.AppData);
            content.Headers.Add("X-API-KEY", _configurationData.ApiKey);
            using var appDataResponse = await _httpClient.PostAsync("Collect/application", content);
            _appDataKey = await appDataResponse.Content.ReadAsStringAsync();
        }

        try
        {
            logInput = logInput with
            {
                AppDataKey = _appDataKey,
                AppData = null
            };

            using var content = JsonContent.Create(logInput);
            content.Headers.Add("X-API-KEY", _configurationData.ApiKey);

            //_logEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, "Post starting.", sw.Elapsed));
            using var response = await _httpClient.PostAsync("Collect", content);
            //_logEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, "Post complete.", sw.Elapsed));

            if (!response.IsSuccessStatusCode)
            {
                //TODO: Consider requeue
                var payload = await response.Content.ReadAsStringAsync();
                var errorMessage = GetErrorMessage(payload);
                _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.CallFailed, logInput, response.StatusCode, errorMessage?.Message ?? response.ReasonPhrase, sw.StopAndGetElapsed()));
            }
            else
            {
                string resourceLocation = null;
                if (response.Headers.TryGetValues("Location", out var locationValues)) resourceLocation = locationValues.FirstOrDefault();
                _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Complete, logInput, response.StatusCode, resourceLocation, sw.StopAndGetElapsed()));
            }
        }
        catch (Exception e)
        {
            //TODO: Consider requeue
            _configurationData.LogEvent?.Invoke(new LogEventArgs(ELogState.Exception, logInput, null, e.Message, sw.StopAndGetElapsed()));
        }
    }

    public async Task UpdateConfigurationAsync(CancellationToken cancellationToken)
    {
        using var content = new HttpRequestMessage(HttpMethod.Get, $"Collect?MinLogLevel={(int)_configurationData.MinLogLevel}");
        content.Headers.Add("X-API-KEY", _configurationData.ApiKey);

        using var result = await _httpClient.SendAsync(content, cancellationToken);
        if (result.IsSuccessStatusCode)
        {
            try
            {
                _configuration = await result.Content.ReadFromJsonAsync<Configuration>(cancellationToken: cancellationToken);
            }
            catch (Exception e)
            {
                Debugger.Break();
            }
        }
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