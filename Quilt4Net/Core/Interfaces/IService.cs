using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Interfaces
{
    public interface IService
    {
        ILog Log { get; }
        Task<ServiceInfoResponse> GetServiceInfo();
    }
}