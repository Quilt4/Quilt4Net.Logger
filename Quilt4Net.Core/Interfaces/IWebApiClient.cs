using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.Events;

namespace Quilt4Net.Core.Interfaces
{
    public class WebApiRequestEventArgs : EventArgs
    {
        internal WebApiRequestEventArgs(Uri baseAddress, string methodName)
        {
        }
    }

    public class WebApiResponseEventArgs : EventArgs
    {
        internal WebApiResponseEventArgs()
        {
        }
    }

    public interface IWebApiClient
    {
        event EventHandler<WebApiRequestEventArgs> WebApiRequestEvent;
        event EventHandler<WebApiResponseEventArgs> WebApiResponseEvent;

        //CRUD style operations
        Task CreateAsync<T>(string controller, T data);
        Task<TResult> CreateAsync<T, TResult>(string controller, T data);
        Task<TResult> ReadAsync<TResult>(string controller, string id);
        Task<IEnumerable<TResult>> ReadAsync<TResult>(string controller);
        Task UpdateAsync<T>(string controller, string id, T data);
        Task DeleteAsync(string controller, string id);

        //CQRS style operations
        Task ExecuteCommandAsync<T>(string controller, string action, T data);
        Task<TResult> ExecuteQueryAsync<T, TResult>(string controller, string action, T data);

        //Authorization
        event EventHandler<AuthorizationChangedEventArgs> AuthorizationChangedEvent;
        void SetAuthorization(string tokenType, string accessToken);
        bool IsAuthorized { get; }
    }
}