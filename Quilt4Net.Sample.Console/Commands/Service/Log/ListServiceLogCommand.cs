using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Service.Log
{
    internal class ListServiceLogCommand : ActionCommandBase
    {
        private readonly IClient _client;

        public ListServiceLogCommand(IClient client)
            : base("List", "List all log entries registered on the server.")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            return _client.Action.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var response = await _client.Action.Service.Log.GetListAsync();
            foreach (var item in response)
            {
                OutputInformation(item.Message.Substring(0, 10));
            }

            return true;
        }
    }
}