using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class GetUserInfoCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public GetUserInfoCommand(IQuilt4NetClient client)
            : base("Info", "Get info about the user.")
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
            var response = _client.Actions.User.GetUserInfoAsync().Result;
            OutputInformation(response.Email);
        }
    }
}