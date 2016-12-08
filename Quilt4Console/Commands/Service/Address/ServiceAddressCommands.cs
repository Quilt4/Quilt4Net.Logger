using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.Service.Address
{
    internal class ServiceAddressCommands : ContainerCommandBase
    {
        public ServiceAddressCommands(IQuilt4NetClient client)
            : base("Address")
        {
            RegisterCommand(new ServiceAddressGetCommand(client));
            RegisterCommand(new ServiceAddressSetCommand(client));
        }
    }
}