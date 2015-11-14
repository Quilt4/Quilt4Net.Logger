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

        public async Task CreateAsync(string username, string password, string email)
        {
            await _webApiClient.ExecuteCommandAsync("user", "Create", new CreateUserRequest { Username = username, Password = password, Email = email });
        }

        public async Task<LoginResponse> Login(string username, string password)
        {
            //TODO: Create keypair on the client, that way we never have to send the private key over the network.
            string publicSessionKey = null;
            string privateSessionKey = null;

            var response = await _webApiClient.ExecuteQueryAsync<LoginRequest, LoginResponse>("user", "Login", new LoginRequest { Username = username, Password = password, PublicSessionKey = publicSessionKey });
            _webApiClient.SetSession(response.PublicSessionKey, response.PrivateSessionKey ?? privateSessionKey);
            return response;
        }
    }
}