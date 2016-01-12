using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal class WebApiClient : IWebApiClient
    {
        private readonly IConfiguration _configuration;
        private Authorization _authorization;

        public event EventHandler<AuthorizationChangedEventArgs> AuthorizationChangedEvent;

        internal WebApiClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public event EventHandler<WebApiRequestEventArgs> WebApiRequestEvent;
        public event EventHandler<WebApiResponseEventArgs> WebApiResponseEvent;

        public async Task CreateAsync<T>(string controller, T data)
        {
            string requestUri = $"api/{controller}";

            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<T>(data, jsonFormatter);

            await Execute(async client =>
                {
                    var response = await client.PostAsync(requestUri, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw await new ExpectedIssues(_configuration).GetExceptionFromResponse(response);
                    }
                });
        }

        public async Task<TResult> CreateAsync<T, TResult>(string controller, T data)
        {
            string requestUri = $"api/{controller}";

            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<T>(data, jsonFormatter);

            var result = await Execute(async client =>
            {
                var response = await client.PostAsync(requestUri, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw await new ExpectedIssues(_configuration).GetExceptionFromResponse(response);
                }

                return response.Content.ReadAsAsync<TResult>().Result;
            });

            return result;
        }

        public async Task<TResult> ReadAsync<TResult>(string controller, string id)
        {
            string requestUri = $"api/{controller}/{id}";

            var result = await Execute(async client =>
                {
                    var response = await client.GetAsync(requestUri);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw await new ExpectedIssues(_configuration).GetExceptionFromResponse(response);
                    }

                    return response.Content.ReadAsAsync<TResult>().Result;
                });
            return result;
        }

        public async Task<IEnumerable<TResult>> ReadAsync<TResult>(string controller)
        {
            string requestUri = $"api/{controller}";

            var result = await Execute(async client =>
            {
                var response = await client.GetAsync(requestUri);
                if (!response.IsSuccessStatusCode)
                {
                    throw await new ExpectedIssues(_configuration).GetExceptionFromResponse(response);
                }

                return response.Content.ReadAsAsync<IEnumerable<TResult>>().Result;
            });
            return result;
        }

        public async Task UpdateAsync<T>(string controller, string id, T data)
        {
            string requestUri = $"api/{controller}/{id}";

            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<T>(data, jsonFormatter);

            await Execute(async client =>
                {
                    await client.PutAsync(requestUri, content);
                });
        }

        public async Task DeleteAsync(string controller, string id)
        {
            string requestUri = $"api/{controller}/{id}";

            await Execute(async client =>
                {
                    var response = await client.DeleteAsync(requestUri);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw await new ExpectedIssues(_configuration).GetExceptionFromResponse(response);
                    }
                });
        }

        public async Task ExecuteCommandAsync<T>(string controller, string action, T data)
        {
            string requestUri = $"api/{controller}/{action}";

            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<T>(data, jsonFormatter);

            await Execute(async client =>
                {
                    var response = await client.PostAsync(requestUri, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw await new ExpectedIssues(_configuration).GetExceptionFromResponse(response);
                    }
                });
        }

        public async Task<TResult> ExecuteQueryAsync<T, TResult>(string controller, string action, T data)
        {
            string requestUri = $"api/{controller}/{action}";

            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<T>(data, jsonFormatter);

            var result = await Execute(async client =>
                {
                    var response = await client.PostAsync(requestUri, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw await new ExpectedIssues(_configuration).GetExceptionFromResponse(response);
                    }

                    return response.Content.ReadAsAsync<TResult>().Result;
                });
            return result;
        }

        public void SetAuthorization(string tokenType, string accessToken)
        {
            _authorization = string.IsNullOrEmpty(accessToken) ? null : new Authorization(tokenType, accessToken);
            OnAuthorizationChangedEvent(new AuthorizationChangedEventArgs(_authorization));
        }

        public bool IsAuthorized => _authorization != null;

        private async Task Execute(Func<HttpClient, Task> action)
        {
            using (var client = GetHttpClient())
            {
                try
                {                    
                    OnWebApiCallEvent(new WebApiRequestEventArgs(client.BaseAddress, action.GetMethodInfo().Name));

                    await action(client);
                }
                catch (TaskCanceledException exception)
                {
                    throw new ExpectedIssues(_configuration).GetException(ExpectedIssues.CallTerminatedByServer, exception);
                }
                finally
                {
                    OnWebApiResponseEvent(new WebApiResponseEventArgs());
                }
            }
        }

        private async Task<T> Execute<T>(Func<HttpClient, Task<T>> action)
        {
            using (var client = GetHttpClient())
            {
                try
                {
                    OnWebApiCallEvent(new WebApiRequestEventArgs(client.BaseAddress, action.GetMethodInfo().Name));

                    var response = await action(client);
                    return response;
                }
                catch (TaskCanceledException exception)
                {
                    throw new ExpectedIssues(_configuration).GetException(ExpectedIssues.CallTerminatedByServer, exception);
                }
                finally
                {
                    OnWebApiResponseEvent(new WebApiResponseEventArgs());
                }
            }
        }

        private HttpClient GetHttpClient()
        {
            var client = new HttpClient { BaseAddress = new Uri(_configuration.Target.Location) };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = _configuration.Target.Timeout;

            //TODO: This is where the hash is supposed to be calculated for the message, so that the server can verify that the origin is correct.
            if (_authorization != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_authorization.TokenType, _authorization.AccessToken);
            }

            return client;
        }

        protected virtual void OnAuthorizationChangedEvent(AuthorizationChangedEventArgs e)
        {
            AuthorizationChangedEvent?.Invoke(this, e);
        }

        protected virtual void OnWebApiCallEvent(WebApiRequestEventArgs e)
        {
            WebApiRequestEvent?.Invoke(this, e);
        }

        protected virtual void OnWebApiResponseEvent(WebApiResponseEventArgs e)
        {
            WebApiResponseEvent?.Invoke(this, e);
        }
    }
}