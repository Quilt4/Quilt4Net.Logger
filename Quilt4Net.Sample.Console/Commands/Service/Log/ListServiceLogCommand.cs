using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.Service.Log
{
    internal class ListServiceLogCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public ListServiceLogCommand(IQuilt4NetClient client)
            : base("List", "List all log entries registered on the server.")
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
            var response = _client.Actions.Service.Log.GetListAsync().Result;
            foreach (var item in response)
            {
                OutputInformation(item.Message);
                OutputInformation(new string('-', 40));
            }
        }
    }
}