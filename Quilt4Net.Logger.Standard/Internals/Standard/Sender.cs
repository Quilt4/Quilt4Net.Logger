using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Quilt4Net.Internals.Standard
{
    public class Sender : ISender
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly Action<LogCompleteEventArgs> _logCompleteEvent;
        private readonly Action<LogFailEventArgs> _logFailEvent;

        public Sender(IConfigurationDataLoader configurationDataLoader, HttpClient httpClient)
        {
            var configuration = configurationDataLoader.Get();
            _httpClient = httpClient;

            _apiKey = configuration.ApiKey;
            _logCompleteEvent = configuration.LogCompleteEvent;
            _logFailEvent = configuration.LogFailEvent;
        }

        public void Send(LogInput logInput)
        {
            Task.Run(async () =>
            {
                var sw = new Stopwatch();
                sw.Start();

                try
                {
                    using (var content = BuildContent(logInput))
                    {
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
                }
                catch (Exception e)
                {
                    _logFailEvent?.Invoke(new LogFailEventArgs(logInput, null, e.Message, sw.StopAndGetElapsed()));
                }
            });
        }

        protected virtual HttpContent BuildContent(LogInput logInput)
        {
            //var type = logInput.GetType();
            //var serializer = new XmlSerializer(type);
            //string value;
            //using (var writer = new StringWriter())
            //{
            //    serializer.Serialize(writer, logInput);
            //    value = writer.ToString();
            //}

            //var content = new StringContent(value, Encoding.UTF8, "application/json");
            //return content;
            throw new NotImplementedException();
        }

        protected virtual ErrorMessage GetErrorMessage(string payload)
        {
            //if (!payload.StartsWith("{")) return null;

            //var serializer = new XmlSerializer(typeof(ErrorMessage));
            //using (var writer = new StringReader(payload))
            //{
            //    var errorMessage = serializer.Deserialize(writer);
            //    return errorMessage as ErrorMessage;
            //}
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}