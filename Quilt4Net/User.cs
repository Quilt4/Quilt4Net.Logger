using System.Threading.Tasks;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net
{
    public class User
    {
        private readonly IWebApiClient _webApiClient;

        internal User(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public async Task CreateAsync(string email, string password)
        {
            await _webApiClient.ExecuteCommandAsync("Account", "Register", new CreateUserRequest { Email = email, Password = password, ConfirmPassword = password });
        }

        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            var response = await _webApiClient.ExecuteQueryAsync<LoginRequest, LoginResponse>("Account", "Login", new LoginRequest { Username = username, Password = password });
            _webApiClient.SetAuthorization(response.token_type, response.access_token);
            return response;
        }
    }
}