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
        private Authorization _authorization;

        public WebApiClient(Uri address, TimeSpan timeout)
        {
            _address = address;
            _timeout = timeout;
        }

        public async Task CreateAsync<T>(string controller, T data)
        {
            string requestUri = $"api/{controller}";

            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<T>(data, jsonFormatter);

            var client = GetHttpClient(requestUri);

            await client.PostAsync(requestUri, content);
        }

        public async Task<IEnumerable<TResult>> ReadAsync<TResult>(string controller)
        {
            string requestUri = $"api/{controller}";

            var client = GetHttpClient(requestUri);
            var response = await client.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException();

            var result = response.Content.ReadAsAsync<IEnumerable<TResult>>().Result;
            return result;
        }

        public async Task<TResult> ReadAsync<TResult>(string controller, string id)
        {
            string requestUri = $"api/{controller}/{id}";

            var client = GetHttpClient(requestUri);
            var response = await client.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException();

            var result = response.Content.ReadAsAsync<TResult>().Result;
            return result;
        }

        public async Task UpdateAsync<T>(string controller, string id, T data)
        {
            string requestUri = $"api/{controller}/{id}";

            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<T>(data, jsonFormatter);

            var client = GetHttpClient(requestUri);

            await client.PutAsync(requestUri, content);
        }

        public async Task DeleteAsync(string controller, string id)
        {
            string requestUri = $"api/{controller}/{id}";

            var client = GetHttpClient(requestUri);

            await client.DeleteAsync(requestUri);
        }

        public async Task ExecuteCommandAsync<T>(string controller, string action, T data)
        {
            string requestUri = $"api/{controller}/{action}";

            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<T>(data, jsonFormatter);

            var client = GetHttpClient(requestUri, content.ToString());
            var response = await client.PostAsync(requestUri, content);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException();
        }

        public async Task<TResult> ExecuteQueryAsync<T, TResult>(string controller, string action, T data)
        {
            string requestUri = $"api/{controller}/{action}";

            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<T>(data, jsonFormatter);

            var client = GetHttpClient(requestUri, content.ToString());
            var response = await client.PostAsync(requestUri, content);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException();

            var result = response.Content.ReadAsAsync<TResult>().Result;
            return result;
        }

        public void SetAuthorization(string tokenType, string accessToken)
        {
            _authorization = new Authorization(tokenType, accessToken);
        }

        private HttpClient GetHttpClient(string requestUri, string content = null)
        {
            var client = new HttpClient { BaseAddress = _address };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = _timeout;

            //This is where the hash is supposed to be calculated for the message
            if (_authorization != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_authorization.TokenType, _authorization.AccessToken);
            }

            return client;
        }
    }
}