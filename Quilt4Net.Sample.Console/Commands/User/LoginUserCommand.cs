using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class LoginUserCommand : ActionCommandBase
    {
        private readonly IClient _client;

        public LoginUserCommand(IClient client)
            : base("Login", "Login user")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            return !_client.Action.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var username = QueryParam<string>("UserName", GetParam(paramList, index++));
            var password = QueryParam<string>("Password", GetParam(paramList, index++));
            await _client.Action.User.LoginAsync(username, password);
            return true;
        }
    }
}