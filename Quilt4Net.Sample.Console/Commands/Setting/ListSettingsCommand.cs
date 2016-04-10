using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Setting
{
    internal class ListSettingsCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public ListSettingsCommand(IQuilt4NetClient client)
            : base("List", "List settings")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            return _client.Actions.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var response = await _client.Actions.ServerSetting.GetListAsync();
            foreach (var item in response)
            {
                OutputInformation("{0}\t{1}",item.Name, item.Value);
            }

            return true;
        }
    }
}