//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Net.Http;
//using System.Net.Http.Formatting;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using Quilt4Net.Core.DataTransfer;
//using Quilt4Net.Core.Events;
//using Quilt4Net.Core.Interfaces;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.WebApi
{
    internal class SignalRClient : IClient
    {
        private readonly BlockingCollection<CommandDto> _commands = new BlockingCollection<CommandDto>();

        public SignalRClient(IConfiguration configuration)
        {
            Task.Run(() =>
            {
                var connection = new HubConnection("http://localhost:8088/");
                connection.StateChanged += change => { Console.WriteLine("StateChanged from " + change.OldState + " to " + change.NewState); };
                var myHub = connection.CreateHubProxy("MyHub");

                while (string.IsNullOrEmpty(connection.ConnectionId))
                {
                    connection.Start().ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            System.Threading.Thread.Sleep(10000); //NOTE: Time to wait between reconnects.
                            Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
                        }
                        else
                        {
                            Console.WriteLine("Connected");
                        }
                    }).Wait();
                }

                //myHub.On<string>("addMessage", param =>
                //{
                //    Console.WriteLine("R2: " + param);
                //});

                while (true)
                {
                    var item = _commands.Take();

                    var success = false;
                    while (!success)
                    {
                        myHub.Invoke<Guid>("Execute", item).ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                            {
                                //TODO: Add a maximum number of retries before failing the command
                                Console.WriteLine("There was an error calling send: {0}", task.Exception.GetBaseException());
                                System.Threading.Thread.Sleep(10000);
                            }
                            else
                            {
                                Console.WriteLine("ServerCommandKey: " + task.Result);
                                success = true;
                            }
                        }).Wait();
                    }
                }
            });
        }

        public void ExecuteCommand(Guid commandKey, ICommand command)
        {
            _commands.Add(new CommandDto
            {
                CommandKey = commandKey,
                Name = command.GetType().Name,
                Data = command,
            });
        }

        public async Task<T> WaitForCommandAsync<T>(Guid commandKey)
        {
            throw new NotImplementedException();
            //TODO: Set a maximum time to wait for response
            //throw new NotImplementedException();

            //return default(T);
        }

        public IEnumerable<ICommand> GetAll()
        {
            throw new NotImplementedException();
        }
    }

    public class CommandDto
    {
        public Guid CommandKey { get; set; }
        public string Name { get; set; }
        public ICommand Data { get; set; }
    }

//    internal class WebApiClient : IWebApiClient
//    {
//        private static int _instanceCounter;
//        private readonly IConfiguration _configuration;
//        private readonly object _syncRoot = new object();
//        private Authorization _authorization;

    //        internal WebApiClient(IConfiguration configuration)
    //        {
    //            lock (_syncRoot)
    //            {
    //                if (_instanceCounter != 0)
    //                    if (!configuration.AllowMultipleInstances)
    //                        throw new InvalidOperationException("Multiple instances is not allowed. Set configuration setting AllowMultipleInstances to true if you want to use multiple instances of this object.");
    //                _instanceCounter++;
    //            }

    //            _configuration = configuration;
    //        }

    //        public event EventHandler<AuthorizationChangedEventArgs> AuthorizationChangedEvent;

    //        public event EventHandler<WebApiRequestEventArgs> WebApiRequestEvent;
    //        public event EventHandler<WebApiResponseEventArgs> WebApiResponseEvent;

    //        public async Task CreateAsync<T>(string controller, T data)
    //        {
    //            await Execute(async client =>
    //            {
    //                await PostAsync(client, $"api/{controller}", data);
    //                return true;
    //            });
    //        }

    //        public async Task<string> PostAsync(string controller, string jsonData)
    //        {
    //            return await Execute(async client =>
    //            {
    //                var response = await PostRawAsync(client, $"api/{controller}", jsonData);
    //                return response.Content.ReadAsStringAsync().Result;
    //            });
    //        }

    //        public async Task<TResult> CreateAsync<T, TResult>(string controller, T data)
    //        {
    //            return await Execute(async client =>
    //            {
    //                var response = await PostAsync(client, $"api/{controller}", data);
    //                return response.Content.ReadAsAsync<TResult>().Result;
    //            });
    //        }

    //        public async Task<TResult> ReadAsync<TResult>(string controller, string id)
    //        {
    //            var result = await Execute(async client =>
    //            {
    //                var response = await GetAsync(client, $"api/{controller}/{id}");
    //                return response.Content.ReadAsAsync<TResult>().Result;
    //            });
    //            return result;
    //        }

    //        public async Task<IEnumerable<TResult>> ReadAsync<TResult>(string controller)
    //        {
    //            var result = await Execute(async client =>
    //            {
    //                var response = await GetAsync(client, $"api/{controller}");
    //                return response.Content.ReadAsAsync<IEnumerable<TResult>>().Result;
    //            });
    //            return result;
    //        }

    //        public async Task UpdateAsync<T>(string controller, string id, T data)
    //        {
    //            await Execute(async client =>
    //            {
    //                await PutAsync(client, $"api/{controller}/{id}", data);
    //                return true;
    //            });
    //        }

    //        public async Task DeleteAsync(string controller, string id)
    //        {
    //            await Execute(async client =>
    //            {
    //                await DeleteAsync(client, $"api/{controller}/{id}");
    //                return true;
    //            });
    //        }

    //        public async Task ExecuteCommandAsync<T>(string controller, string action, T data)
    //        {
    //            await Execute(async client =>
    //            {
    //                await PostAsync(client, $"api/{controller}/{action}", data);
    //                return true;
    //            });
    //        }

    //        public async Task<TResult> ExecuteQueryAsync<T, TResult>(string controller)
    //        {
    //            var result = await Execute(async client =>
    //            {
    //                var response = await GetAsync(client, $"api/{controller}");
    //                var output = response.Content.ReadAsAsync<TResult>().Result;
    //                return output;
    //            });
    //            return result;
    //        }

    //        public async Task<TResult> ExecuteQueryAsync<T, TResult>(string controller, string id)
    //        {
    //            return await ExecuteQueryAsync<T, TResult>(controller + "/" + id);
    //        }

    //        public async Task<TResult> PostQueryAsync<TResult>(string controller, string action, FormUrlEncodedContent cnt)
    //        {
    //            var result = await Execute(async client =>
    //            {
    //                var response = await PostAsync(client, $"api/{controller}/{action}", cnt);
    //                return response.Content.ReadAsAsync<TResult>().Result;
    //            });
    //            return result;
    //        }

    //        public async Task<TResult> PostQueryAsync<T, TResult>(string controller, string action, T data)
    //        {
    //            var result = await Execute(async client =>
    //            {
    //                var response = await PostAsync(client, $"api/{controller}/{action}", data);
    //                return response.Content.ReadAsAsync<TResult>().Result;
    //            });
    //            return result;
    //        }

    //        public void SetAuthorization(string userName, string tokenType, string accessToken)
    //        {
    //            _authorization = string.IsNullOrEmpty(accessToken) ? null : new Authorization(tokenType, accessToken);
    //            OnAuthorizationChangedEvent(new AuthorizationChangedEventArgs(userName, _authorization));
    //        }

    //        public bool IsAuthorized => _authorization != null;

    //        private async Task<HttpResponseMessage> PostRawAsync(HttpClient client, string requestUri, string jsonData)
    //        {
    //            return await PostAsync(client, requestUri, () => new Tuple<HttpContent, string>(new StringContent(jsonData, Encoding.UTF8, "application/json"), jsonData));
    //        }

    //        private async Task<HttpResponseMessage> PostAsync<T>(HttpClient client, string requestUri, T data)
    //        {
    //            return await PostAsync(client, requestUri, () =>
    //            {
    //                var jsonFormatter = new JsonMediaTypeFormatter();
    //                var content = new ObjectContent<T>(data, jsonFormatter);
    //                var serializeObject = JsonConvert.SerializeObject(data);
    //                return new Tuple<HttpContent, string>(content, serializeObject);
    //            });
    //        }

    //        private async Task<HttpResponseMessage> PostAsync(HttpClient client, string requestUri, Func<Tuple<HttpContent, string>> buildContent)
    //        {
    //            WebApiRequestEventArgs request = null;
    //            try
    //            {
    //                var content = buildContent();
    //                request = new WebApiRequestEventArgs(client.BaseAddress, requestUri, OperationType.Post, content.Item2);

    //                OnWebApiRequestEvent(request);

    //                var response = Task.Run(() =>
    //                {
    //                    try
    //                    {
    //                        return client.PostAsync(requestUri, content.Item1);
    //                    }
    //                    catch (Exception exception)
    //                    {
    //                        Debug.WriteLine(exception.Message);
    //                        throw;
    //                    }
    //                });

    //                if (!response.Wait(client.Timeout))
    //                    throw new TimeoutException();

    //                if (!response.Result.IsSuccessStatusCode)
    //                    throw await new ExpectedIssues(_configuration).GetExceptionFromResponse(response.Result);

    //                var contentData = response.Result.Content.ReadAsStringAsync().Result;
    //                OnWebApiResponseEvent(new WebApiResponseEventArgs(request, response.Result, contentData));
    //                return response.Result;
    //            }
    //            catch (Exception exception)
    //            {
    //                OnWebApiResponseEvent(new WebApiResponseEventArgs(request, exception));
    //                throw;
    //            }
    //        }

    //        private async Task<HttpResponseMessage> GetAsync(HttpClient client, string requestUri)
    //        {
    //            WebApiRequestEventArgs request = null;
    //            try
    //            {
    //                request = new WebApiRequestEventArgs(client.BaseAddress, requestUri, OperationType.Get);
    //                OnWebApiRequestEvent(request);

    //                var response = await client.GetAsync(requestUri);
    //                if (!response.IsSuccessStatusCode)
    //                    throw await new ExpectedIssues(_configuration).GetExceptionFromResponse(response);

    //                var contentData = response.Content.ReadAsStringAsync().Result;
    //                OnWebApiResponseEvent(new WebApiResponseEventArgs(request, response, contentData));
    //                return response;
    //            }
    //            catch (Exception exception)
    //            {
    //                OnWebApiResponseEvent(new WebApiResponseEventArgs(request, exception));
    //                throw;
    //            }
    //        }

    //        private async Task PutAsync<T>(HttpClient client, string requestUri, T data)
    //        {
    //            WebApiRequestEventArgs request = null;
    //            try
    //            {
    //                var jsonFormatter = new JsonMediaTypeFormatter();
    //                var content = new ObjectContent<T>(data, jsonFormatter);

    //                request = new WebApiRequestEventArgs(client.BaseAddress, requestUri, OperationType.Put, JsonConvert.SerializeObject(data));
    //                OnWebApiRequestEvent(request);

    //                var response = await client.PutAsync(requestUri, content);
    //                if (!response.IsSuccessStatusCode)
    //                    throw await new ExpectedIssues(_configuration).GetExceptionFromResponse(response);

    //                OnWebApiResponseEvent(new WebApiResponseEventArgs(request, response));
    //            }
    //            catch (Exception exception)
    //            {
    //                OnWebApiResponseEvent(new WebApiResponseEventArgs(request, exception));
    //                throw;
    //            }
    //        }

    //        private async Task DeleteAsync(HttpClient client, string requestUri)
    //        {
    //            WebApiRequestEventArgs request = null;
    //            try
    //            {
    //                request = new WebApiRequestEventArgs(client.BaseAddress, requestUri, OperationType.Delete);
    //                OnWebApiRequestEvent(request);

    //                var response = await client.DeleteAsync(requestUri);
    //                if (!response.IsSuccessStatusCode)
    //                    throw await new ExpectedIssues(_configuration).GetExceptionFromResponse(response);

    //                OnWebApiResponseEvent(new WebApiResponseEventArgs(request, response));
    //            }
    //            catch (Exception exception)
    //            {
    //                OnWebApiResponseEvent(new WebApiResponseEventArgs(request, exception));
    //                throw;
    //            }
    //        }

    //        private async Task<T> Execute<T>(Func<HttpClient, Task<T>> action)
    //        {
    //            using (var client = GetHttpClient())
    //            {
    //                try
    //                {
    //                    var response = await action(client);
    //                    return response;
    //                }
    //                catch (TaskCanceledException exception)
    //                {
    //                    throw new ExpectedIssues(_configuration).GetException(ExpectedIssues.CallTerminatedByServer, exception);
    //                }
    //            }
    //        }

    //        private HttpClient GetHttpClient()
    //        {
    //            var client = new HttpClient { BaseAddress = new Uri(_configuration.Target.Location) };
    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //            client.Timeout = _configuration.Target.Timeout;

    //            //TODO: This is where the hash is supposed to be calculated for the message, so that the server can verify that the origin is correct.
    //            if (_authorization != null)
    //                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_authorization.TokenType, _authorization.AccessToken);

    //            return client;
    //        }

    //        protected virtual void OnAuthorizationChangedEvent(AuthorizationChangedEventArgs e)
    //        {
    //            AuthorizationChangedEvent?.Invoke(this, e);
    //        }

    //        protected virtual void OnWebApiRequestEvent(WebApiRequestEventArgs e)
    //        {
    //            WebApiRequestEvent?.Invoke(this, e);
    //        }

    //        protected virtual void OnWebApiResponseEvent(WebApiResponseEventArgs e)
    //        {
    //            WebApiResponseEvent?.Invoke(this, e);
    //        }
    //    }
}