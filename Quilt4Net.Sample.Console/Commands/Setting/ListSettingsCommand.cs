using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Setting
{
    internal class ListSettingsCommand : ActionCommandBase
    {
        private readonly IClient _client;

        public ListSettingsCommand(IClient client)
            : base("List", "List settings")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var response = await _client.Action.ServerSetting.GetListAsync();
            foreach (var item in response)
            {
                OutputInformation("{0}\t{1}",item.Name, item.Value);
            }

            return true;
        }
    }
}