using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tharga.Quilt4Net.Interfaces
{
    public interface IWebApiClient
    {
        Task<TResult> ExecuteGet<T, TResult>(string controller, string id);
        Task<IEnumerable<TResult>> ExecuteGetList<TResult>(string controller);
        Task ExecuteCreateCommandAsync<T>(string controller, T data);
        Task ExecuteCommandAsync<T>(string controller, string action, T data);
        Task<TResult> ExecuteQueryAsync<T, TResult>(string controller, string action, T data);
        void SetSession(string publicSessionKey, string privateSessionKey);
    }
}