using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class GetUserInfoCommand : ActionCommandBase
    {
        private readonly IClient _client;

        public GetUserInfoCommand(IClient client)
            : base("Info", "Get info about the user.")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            return _client.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var response = await _client.User.GetUserInfoAsync();
            OutputInformation("{0}",response.Email);
            return true;
        }
    }
}