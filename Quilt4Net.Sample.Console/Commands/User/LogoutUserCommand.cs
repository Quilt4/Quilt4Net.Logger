using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

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

        public override bool CanExecute()
        {
            return _client.Actions.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            await _client.Actions.User.LogoutAsync();
            return true;
        }
    }
}