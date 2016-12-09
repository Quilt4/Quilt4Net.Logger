using Quilt4Console.Commands.Service.Address;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.Service
{
    internal class ServiceCommands : ContainerCommandBase
    {
        public ServiceCommands(IQuilt4NetClient client)
            : base("Service")
        {
            RegisterCommand(new ServiceAddressCommands(client));
            //RegisterCommand(new ServiceApiKeyCommands(client));
            RegisterCommand(new ServiceInfoCommand(client));
            //RegisterCommand(new ServiceLogCommands(client));
            //RegisterCommand(new ServicePostCommand(client));
        }
    }
}