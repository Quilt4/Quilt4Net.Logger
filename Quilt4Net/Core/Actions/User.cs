using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.Actions
{
    public class User : IUser
    {
        private readonly IWebApiClient _webApiClient;

        internal User(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public bool IsAuthorized => _webApiClient.IsAuthorized;

        public async Task CreateAsync(string userName, string email, string fullName, string password)
        {
            if (fullName.Length < 2) throw new ArgumentException("Parameter full name must be at least 2 characters long.");
            if (fullName.Length > 100) throw new ArgumentException("Parameter full name cannot be more than 100 characters long.");

            await _webApiClient.ExecuteCommandAsync("Account", "Register", new RegisterBindingModel { UserName = userName, Email = email, FullName = fullName, Password = password, ConfirmPassword = password });
        }

        public async Task<ILoginResult> LoginAsync(string username, string password)
        {
            var response = await _webApiClient.PostQueryAsync<LoginData, LoginResult>("Account", "Login", new LoginData { username = username, password = password });
            _webApiClient.SetAuthorization(username, response.token_type, response.access_token);
            return response;
        }

        public async Task LogoutAsync()
        {
            await _webApiClient.ExecuteCommandAsync<string>("Account", "Logout", null);
            _webApiClient.SetAuthorization(null, null, null);
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
            return await _webApiClient.ReadAsync<UserResponse>("User");
        }

        public async Task<IEnumerable<QueryUserResponse>> SearchAsync(string searchString)
        {
            return await _webApiClient.ExecuteQueryAsync<QueryUserRequest, IEnumerable<QueryUserResponse>>("User", searchString);
        }
    }
}