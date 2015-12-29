using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Interfaces
{
    public interface IUser
    {
        bool IsAuthorized { get; }
        Task CreateAsync(string userName, string email, string password);
        Task<ILoginResult> LoginAsync(string username, string password);
        Task LogoutAsync();
        Task<UserInfoViewModel> GetUserInfoAsync();
        Task ChangePassword(string oldPassword, string newPassword, string confirmPassword);
        Task AddRoleAsync(string userName, string role);
        Task<IEnumerable<UserResponse>> GetListAsync();
    }
}