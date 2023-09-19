using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace Quilt4Net.Internals;

internal class Sender : ISender
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly Action<LogCompleteEventArgs> _logCompleteEvent;
    private readonly Action<LogFailEventArgs> _logFailEvent;

    public Sender(IConfigurationDataLoader configurationDataLoader, HttpClient httpClient)
    {
        var configuration = configurationDataLoader.Get();
        _httpClient = httpClient;

        SetBaseAddress(configuration);

        _apiKey = configuration.ApiKey;
        _logCompleteEvent = configuration.LogCompleteEvent;
        _logFailEvent = configuration.LogFailEvent;
    }

    private void SetBaseAddress(ConfigurationData configuration)
    {
        var baseAddress = configuration.BaseAddress;
        if (!baseAddress.EndsWith("/")) baseAddress += "/";
        if (!Uri.TryCreate(baseAddress, UriKind.Absolute, out var address)) throw new InvalidOperationException($"Cannot parse '{baseAddress}' to an absolute uri.");
        _httpClient.BaseAddress = address;
    }

    public void Send(LogInput logInput)
    {
        Task.Run(async () =>
        {
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                var content = BuildContent(logInput);
                content.Headers.Add("X-API-KEY", _apiKey);

                var response = await _httpClient.PostAsync("Collect", content);
                if (!response.IsSuccessStatusCode)
                {
                    var payload = await response.Content.ReadAsStringAsync();
                    var errorMessage = GetErrorMessage(payload);
                    _logFailEvent?.Invoke(new LogFailEventArgs(logInput, response.StatusCode, errorMessage?.Message ?? response.ReasonPhrase, sw.StopAndGetElapsed()));
                }
                else
                {
                    string resourceLocation = null;
                    if (response.Headers.TryGetValues("Location", out var locationValues)) resourceLocation = locationValues.FirstOrDefault();
                    _logCompleteEvent?.Invoke(new LogCompleteEventArgs(logInput, resourceLocation, sw.StopAndGetElapsed()));
                }
            }
            catch (Exception e)
            {
                _logFailEvent?.Invoke(new LogFailEventArgs(logInput, null, e.Message, sw.StopAndGetElapsed()));
            }
        });
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