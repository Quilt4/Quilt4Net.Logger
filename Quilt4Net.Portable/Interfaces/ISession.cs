using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Interfaces
{
    public interface ISession
    {
        bool IsRegistered { get; }
        Task RegisterAsync(string projectApiKey, string environment);
        Task RegisterAsync(string projectApiKey, string environment, Assembly firstAssembly);
        Task<IEnumerable<SessionData>> GetListAsync();
    }
}