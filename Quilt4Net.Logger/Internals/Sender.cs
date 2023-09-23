using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace Quilt4Net.Internals;

internal class Sender : ISender
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly Action<LogEventArgs> _logEvent;

    public Sender(IConfigurationDataLoader configurationDataLoader)
    {
        var configuration = configurationDataLoader.Get();
        _httpClient = new HttpClient();

        SetBaseAddress(configuration.BaseAddress, _httpClient);

        _apiKey = configuration.ApiKey;
        _logEvent = configuration.LogEvent;
    }

    private void SetBaseAddress(string baseAddress, HttpClient httpClient)
    {
        if (!baseAddress.EndsWith("/")) baseAddress += "/";
        if (!Uri.TryCreate(baseAddress, UriKind.Absolute, out var address)) throw new InvalidOperationException($"Cannot parse '{baseAddress}' to an absolute uri.");
        httpClient.BaseAddress = address;
    }

    public void Send(LogInput logInput)
    {
        Task.Run(async () =>
        {
            var sw = new Stopwatch();
            sw.Start();
            //_logEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, "Initiate send.", sw.Elapsed));

            try
            {
                var content = BuildContent(logInput);
                content.Headers.Add("X-API-KEY", _apiKey);

                //_logEvent?.Invoke(new LogEventArgs(ELogState.Debug, logInput, null, "Post starting.", sw.Elapsed));
                var response = await _httpClient.PostAsync("Collect", content);
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

    public async Task GetConfigurationAsync(CancellationToken cancellationToken)
    {
        var content = new HttpRequestMessage(HttpMethod.Get, "Collect");
        content.Headers.Add("X-API-KEY", _apiKey);

        var result = await _httpClient.SendAsync(content, cancellationToken);
        if (result.IsSuccessStatusCode)
        {
            var configurations = await result.Content.ReadFromJsonAsync<Configuration[]>(cancellationToken: cancellationToken);
        }
    }

    protected virtual HttpContent BuildContent(LogInput logInput)
    {
        var content = JsonContent.Create(logInput);
        return content;
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
        _httpClient?.Dispose();
    }
}