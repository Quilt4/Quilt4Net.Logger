using System;
using System.Collections.Generic;
//using System.Net.Http;
using System.Threading.Tasks;
using Quilt4Net.Core.Events;

namespace Quilt4Net.Core.Interfaces
{
    public interface ICommand
    {
    }

    public interface IClient
    {
        void ExecuteCommand(Guid commandKey, ICommand command);
        Task<T> WaitForCommandAsync<T>(Guid commandKey);
        IEnumerable<ICommand> GetAll();
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
        Task<TResult> ExecuteQueryAsync<T, TResult>(string controller);
        Task<TResult> ExecuteQueryAsync<T, TResult>(string controller, string id);
        Task<TResult> PostQueryAsync<T, TResult>(string controller, string action, T data);
        //Task<TResult> PostQueryAsync<TResult>(string controller, string action, FormUrlEncodedContent cnt);

        //Basic operations
        Task<string> PostAsync(string controller, string jsonData);

        //Authorization
        event EventHandler<AuthorizationChangedEventArgs> AuthorizationChangedEvent;
        void SetAuthorization(string userName, string tokenType, string accessToken);
        bool IsAuthorized { get; }
    }
}