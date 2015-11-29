using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tharga.Quilt4Net.Interfaces
{
    public interface IWebApiClient
    {
        Task<IEnumerable<TResult>> ExecuteGet<TResult>(string controller, string action);
        Task ExecuteCommandAsync<T>(string controller, string action, T data);

        //TODO: Revisit
        Task<TResult> ExecuteGet<T, TResult>(string controller, string id);
        Task<TResult> ExecuteQueryAsync<T, TResult>(string controller, string action, T data);
        void SetAuthorization(string tokenType, string accessToken);
    }
}