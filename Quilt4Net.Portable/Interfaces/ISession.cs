using System.Reflection;
using System.Threading.Tasks;

namespace Quilt4Net.Core.Interfaces
{
    public interface ISession
    {
        Task RegisterAsync(string projectApiKey, string environment);
        Task RegisterAsync(string projectApiKey, string environment, Assembly firstAssembly);
    }
}