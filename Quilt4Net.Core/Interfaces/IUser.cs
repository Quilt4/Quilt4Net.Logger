using System.Threading.Tasks;

namespace Quilt4Net.Core.Interfaces
{
    public interface IUser
    {
        bool IsAuthorized { get; }
        Task CreateAsync(string userName, string email, string password);
        Task<ILoginResult> LoginAsync(string username, string password);
        Task LogoutAsync();
    }
}