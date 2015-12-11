using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public class User
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

        public async Task<LoginData> LoginAsync(string username, string password)
        {
            var response = await _webApiClient.ExecuteQueryAsync<LoginRequest, LoginData>("Account", "Login", new LoginRequest { Username = username, Password = password });
            _webApiClient.SetAuthorization(response.token_type, response.access_token);
            return response;
        }

        public async Task LogoutAsync()
        {
            await Task.Run(() => _webApiClient.SetAuthorization(null, null));
        }
    }
}