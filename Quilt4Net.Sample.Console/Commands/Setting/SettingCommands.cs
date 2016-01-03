using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Setting
{
    internal class SettingCommands : ContainerCommandBase
    {
        public SettingCommands(Client client)
            : base("Setting")
        {
            RegisterCommand(new ListSettingsCommand(client));
        }
    }

    internal class ListSettingsCommand : ActionCommandBase
    {
        private readonly Client _client;

        public ListSettingsCommand(Client client)
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