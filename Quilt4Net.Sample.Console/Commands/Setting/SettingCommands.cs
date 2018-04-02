using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.Setting
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