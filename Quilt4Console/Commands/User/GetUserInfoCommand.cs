using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.User
{
    internal class GetUserInfoCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public GetUserInfoCommand(IQuilt4NetClient client)
            : base("Info", "Get info about the user.")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            return _client.Actions.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var response = await _client.Actions.User.GetUserInfoAsync();
            OutputInformation("{0}", response.Email);
            return true;
        }
    }
}