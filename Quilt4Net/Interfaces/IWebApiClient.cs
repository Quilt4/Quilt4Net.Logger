using System.Threading.Tasks;

namespace Tharga.Quilt4Net
{
    public interface IWebApiClient
    {
        Task ExecuteCommandAsync<T>(string controller, string action, T data);
        Task<TResult> ExecuteQueryAsync<T, TResult>(string controller, string action, T data);
    }
}