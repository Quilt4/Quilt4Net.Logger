using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net
{
    public class WebApiClient : IWebApiClient
    {
        private readonly Uri _address;
        private readonly TimeSpan _timeout;
        private Tuple<string,string> _keyPair;

        public WebApiClient(Uri address, TimeSpan timeout)
        {
            _address = address;
            _timeout = timeout;
        }

        public async Task<TResult> ExecuteGet<T, TResult>(string controller, string id)
        {
            string requestUri = $"api/{controller}/{id}";

            var client = GetHttpClient(requestUri);
            var response = await client.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException();

            var result = response.Content.ReadAsAsync<TResult>().Result;
            return result;
        }

        public async Task<IEnumerable<TResult>> ExecuteGetList<TResult>(string controller)
        {
            string requestUri = $"api/{controller}";

            var client = GetHttpClient(requestUri);
            var response = await client.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException();

            var result = response.Content.ReadAsAsync<IEnumerable<TResult>>().Result;
            return result;
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

        public void SetSession(string publicSessionKey, string privateSessionKey)
        {
            _keyPair = new Tuple<string, string>(publicSessionKey, privateSessionKey);
        }

        public async Task ExecuteCreateCommandAsync<T>(string controller, T data)
        {
            string requestUri = $"api/{controller}";

            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<T>(data, jsonFormatter);

            var client = GetHttpClient(requestUri, content.ToString());
            var response = await client.PostAsync(requestUri, content);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException();
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

        private HttpClient GetHttpClient(string requestUri, string content = null)
        {
            var client = new HttpClient { BaseAddress = _address };
            //var client = new WebClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = _timeout;

            //This is where the hash is supposed to be calculated for the message
            if (_keyPair != null)
            {
                var rsaProvider = new RSACryptoServiceProvider(512);
                //TODO: Use the private key to create a signature for the requestUri and content
                //requestUri
                //content

                var messageHash = "ABC";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_keyPair.Item1, Convert.ToBase64String(Encoding.UTF8.GetBytes(messageHash)));
            }

            return client;
        }
    }
}