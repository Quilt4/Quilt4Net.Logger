using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class LoginUserCommand : ActionCommandBase
    {
        private readonly Client _client;

        public LoginUserCommand(Client client)
            : base("Login", "Login user")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            return !_client.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var username = QueryParam<string>("UserName", GetParam(paramList, index++));
            var password = QueryParam<string>("Password", GetParam(paramList, index++));
            await _client.User.LoginAsync(username, password);
            return true;
        }
    }
}