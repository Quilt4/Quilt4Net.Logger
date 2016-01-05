using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

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

        public override bool CanExecute()
        {
            return _client.Actions.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var response = await _client.Actions.Service.Log.GetListAsync();
            foreach (var item in response)
            {
                OutputInformation(item.Message.Substring(0, 10));
            }

            return true;
        }
    }
}