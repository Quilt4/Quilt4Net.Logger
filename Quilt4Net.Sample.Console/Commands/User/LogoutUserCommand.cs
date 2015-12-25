using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class LogoutUserCommand : ActionCommandBase
    {
        private readonly IClient _client;

        public LogoutUserCommand(IClient client)
            : base("Logout", "Logout user")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            return _client.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            await _client.User.LogoutAsync();
            return true;
        }
    }
}