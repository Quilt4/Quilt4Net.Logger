using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Quilt4Net.Internals;

internal class Sender : ISender
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly Action<LogEventArgs> _logEvent;
    private readonly LogLevel _minLogLevel;
    private Configuration _configuration = new ();

    public Sender(IConfigurationDataLoader configurationDataLoader)
    {
        _httpClient = new HttpClient();
        var configuration = configurationDataLoader.Get();

        SetBaseAddress(configuration.BaseAddress, _httpClient);

        _apiKey = configuration.ApiKey;
        _logEvent = configuration.LogEvent;
        _minLogLevel = configuration.MinLogLevel;
    }

    private void SetBaseAddress(string baseAddress, HttpClient httpClient)
    {
        if (!baseAddress.EndsWith("/")) baseAddress += "/";
        if (!Uri.TryCreate(baseAddress, UriKind.Absolute, out var address)) throw new InvalidOperationException($"Cannot parse '{baseAddress}' to an absolute uri.");
        httpClient.BaseAddress = address;
    }

    public void Send(LogInput logInput)
    {
        if (_configuration?.LogLevel != null && logInput.LogLevel < _configuration.LogLevel)
        {
            _logEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, $"Skip because only logging {_configuration.LogLevel} and above, this message was {logInput.LogLevel}.", TimeSpan.Zero));
            return;
        }

        Task.Run(async () =>
        {
            var sw = new Stopwatch();
            sw.Start();
            //_logEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, "Initiate send.", sw.Elapsed));

            try
            {
                using var content = JsonContent.Create(logInput);
                content.Headers.Add("X-API-KEY", _apiKey);

                //_logEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, "Post starting.", sw.Elapsed));
                //using var httpClient = _httpClientFactory.CreateClient("Quilt4Net.Sender");
                using var response = await _httpClient.PostAsync("Collect", content);
                //var response = await _httpClient.PostAsync("Collect", content);
                //_logEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, "Post complete.", sw.Elapsed));

                if (!response.IsSuccessStatusCode)
                {
                    var payload = await response.Content.ReadAsStringAsync();
                    var errorMessage = GetErrorMessage(payload);
                    _logEvent?.Invoke(new LogEventArgs(ELogState.CallFailed, logInput, response.StatusCode, errorMessage?.Message ?? response.ReasonPhrase, sw.StopAndGetElapsed()));
                }
                else
                {
                    string resourceLocation = null;
                    if (response.Headers.TryGetValues("Location", out var locationValues)) resourceLocation = locationValues.FirstOrDefault();
                    _logEvent?.Invoke(new LogEventArgs(ELogState.Complete, logInput, response.StatusCode, resourceLocation, sw.StopAndGetElapsed()));
                }
            }
            catch (Exception e)
            {
                _logEvent?.Invoke(new LogEventArgs(ELogState.Exception, logInput, null, e.Message, sw.StopAndGetElapsed()));
            }
        });
    }

    public async Task UpdateConfigurationAsync(CancellationToken cancellationToken)
    {
        using var content = new HttpRequestMessage(HttpMethod.Get, $"Collect?MinLogLevel={(int)_minLogLevel}");
        content.Headers.Add("X-API-KEY", _apiKey);

        //using var httpClient = _httpClientFactory.CreateClient("Quilt4Net.Sender");
        using var result = await _httpClient.SendAsync(content, cancellationToken);
        if (result.IsSuccessStatusCode)
        {
            _configuration = await result.Content.ReadFromJsonAsync<Configuration>(cancellationToken: cancellationToken);
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
        // TODO release managed resources here
    }
}