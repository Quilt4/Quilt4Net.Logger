using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.User
{
    internal class LoginUserCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public LoginUserCommand(IQuilt4NetClient client)
            : base("Login", "Login user.")
        {
            _client = client;
        }

        public override bool CanExecute(out string reasonMessage)
        {
            reasonMessage = "Already logged on.";
            return !_client.Actions.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var username = QueryParam<string>("UserName", GetParam(paramList, index++));
            var password = QueryParam<string>("Password", GetParam(paramList, index++));
            await _client.Actions.User.LoginAsync(username, password);
            return true;
        }
    }
}