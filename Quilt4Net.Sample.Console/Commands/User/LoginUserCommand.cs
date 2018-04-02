using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class LoginUserCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public LoginUserCommand(IQuilt4NetClient client)
            : base("Login", "Login user")
        {
            _client = client;
        }

        public override bool CanExecute(out string reasonMessage)
        {
            reasonMessage = string.Empty;
            if (_client.Actions.User.IsAuthorized)
                reasonMessage = "Authorized";
            return !_client.Actions.User.IsAuthorized;
        }

        public override void Invoke(string[] param)
        {
            var username = QueryParam<string>("UserName", param);
            var password = QueryParam<string>("Password", param);
            _client.Actions.User.LoginAsync(username, password);
        }
    }
}