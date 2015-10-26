using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tharga.Quilt4Net.Interfaces;

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

        public async Task<TResult> ExecuteGet<T, TResult>(string controller, string id)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"api/{controller}/{id}");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException();

            var result = response.Content.ReadAsAsync<TResult>().Result;
            return result;
        }

        public async Task<IEnumerable<TResult>> ExecuteGetList<TResult>(string controller)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"api/{controller}");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException();

            var result = response.Content.ReadAsAsync<IEnumerable<TResult>>().Result;
            return result;
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

        public async Task ExecuteCreateCommandAsync<T>(string controller, T data)
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<T>(data, jsonFormatter);

            var client = GetHttpClient();
            var response = await client.PostAsync($"api/{controller}", content);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException();
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