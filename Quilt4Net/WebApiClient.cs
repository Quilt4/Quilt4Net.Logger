using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Tharga.Quilt4Net
{
    public class WebApiClient : IWebApiClient
    {
        private readonly Uri _address;
        private readonly TimeSpan _timeout;

        public WebApiClient(Uri address, TimeSpan timeout)
        {
            _address = address;
            _timeout = timeout;
        }

        public async Task<TResult> ExecuteQueryAsync<T, TResult>(string controller, string action, T data)
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<T>(data, jsonFormatter);

            var client = GetHttpClient();
            var response = await client.PostAsync($"api/{controller}/{action}", content);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException();

            var result = response.Content.ReadAsAsync<TResult>().Result;
            return result;
        }

        public async Task ExecuteCommandAsync<T>(string controller, string action, T data)
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<T>(data, jsonFormatter);

            var client = GetHttpClient();
            var response = await client.PostAsync($"api/{controller}/{action}", content);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException();
        }

        private HttpClient GetHttpClient()
        {
            var client = new HttpClient { BaseAddress = _address };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = _timeout;
            return client;
        }
    }
}