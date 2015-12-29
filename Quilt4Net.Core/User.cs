using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public class User : IUser
    {
        private readonly IWebApiClient _webApiClient;

        internal User(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public bool IsAuthorized => _webApiClient.IsAuthorized;

        public async Task CreateAsync(string userName, string email, string password)
        {
            await _webApiClient.ExecuteCommandAsync("Account", "Register", new RegisterBindingModel { UserName = userName, Email = email, Password = password, ConfirmPassword = password });
        }

        public async Task<ILoginResult> LoginAsync(string username, string password)
        {
            var response = await _webApiClient.ExecuteQueryAsync<LoginData, LoginResult>("Account", "Login", new LoginData { Username = username, Password = password });
            _webApiClient.SetAuthorization(response.token_type, response.access_token);
            return response;
        }

        public async Task LogoutAsync()
        {
            await _webApiClient.ExecuteCommandAsync<string>("Account", "Logout", null);
            _webApiClient.SetAuthorization(null, null);
        }

        public async Task<UserInfoViewModel> GetUserInfoAsync()
        {
            var response = await _webApiClient.ReadAsync<UserInfoViewModel>("Account", "UserInfo");
            return response;
        }

        public async Task ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            await _webApiClient.ExecuteCommandAsync("Account", "ChangePassword", new ChangePasswordBindingModel { OldPassword = oldPassword, NewPassword = newPassword, ConfirmPassword = confirmPassword });
        }

        public async Task AddRoleAsync(string userName, string role)
        {
            await _webApiClient.ExecuteCommandAsync("Account", "Role/Assign", new AddRoleModel { UserName = userName, Role = role });
        }

        public async Task<IEnumerable<UserResponse>> GetListAsync()
        {
            return await _webApiClient.ReadAsync<UserResponse>("Client/User");
        }
    }
}