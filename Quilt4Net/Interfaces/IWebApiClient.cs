using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tharga.Quilt4Net.Interfaces
{
    public interface IWebApiClient
    {
        //CRUD style operations
        Task CreateAsync<T>(string controller, T data);
        Task<IEnumerable<TResult>> ReadAsync<TResult>(string controller);
        Task<TResult> ReadAsync<TResult>(string controller, string id);
        Task UpdateAsync<T>(string controller, string id, T data);
        Task DeleteAsync(string controller, string id);

        //CQRS style operations
        Task ExecuteCommandAsync<T>(string controller, string action, T data);
        Task<TResult> ExecuteQueryAsync<T, TResult>(string controller, string action, T data);
        void SetAuthorization(string tokenType, string accessToken);
    }
}