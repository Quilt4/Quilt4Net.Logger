using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Interfaces
{
    public interface ISession
    {
        bool IsRegistered { get; }
        Task RegisterAsync();
        Task<IEnumerable<SessionData>> GetListAsync();
    }
}