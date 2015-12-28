using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Service.Log
{
    internal class ServiceLogCommands : ContainerCommandBase
    {
        public ServiceLogCommands(IClient client)
            : base("Log")
        {
            RegisterCommand(new ListServiceLogCommand(client));
        }
    }
}
