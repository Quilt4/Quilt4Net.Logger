using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class LogoutUserCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public LogoutUserCommand(IQuilt4NetClient client)
            : base("Logout", "Logout user")
        {
            _client = client;
        }

        public override bool CanExecute(out string reasonMessage)
        {
            reasonMessage = string.Empty;
            if (!_client.Actions.User.IsAuthorized)
                reasonMessage = "Not Authorized";
            return _client.Actions.User.IsAuthorized;
        }

        public override void Invoke(string[] param)
        {
            _client.Actions.User.LogoutAsync();
        }
    }
}