using Quilt4Net.Core.Interfaces;
using Quilt4Net.Sample.Console.Commands.Service.Log;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Service
{
    internal class ServiceCommands : ContainerCommandBase
    {
        public ServiceCommands(IClient client)
            : base("Service")
        {
            RegisterCommand(new ServiceLogCommands(client));
        }
    }
}