using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.Setting
{
    internal class SettingCommands : ContainerCommandBase
    {
        public SettingCommands(IQuilt4NetClient client)
            : base("Setting")
        {
            RegisterCommand(new ListSettingsCommand(client));
        }
    }
}
