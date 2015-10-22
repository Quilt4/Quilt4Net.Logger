using System.Threading.Tasks;
using Tharga.Quilt4Net.DataTransfer;

namespace Tharga.Quilt4Net
{
    public class User
    {
        private readonly IWebApiClient _webApiClient;

        internal User(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public async Task CreateAsync(string username, string password, string email)
        {
            await _webApiClient.ExecuteCommandAsync("user", "Create", new CreateUserRequest { Username = username, Password = password, Email = email });
        }

        public async Task<LoginResponse> Login(string username, string password)
        {
            var response = await _webApiClient.ExecuteQueryAsync<LoginRequest, LoginResponse>("user", "Login", new LoginRequest { Username = username, Password = password });
            return response;
        }
    }
}