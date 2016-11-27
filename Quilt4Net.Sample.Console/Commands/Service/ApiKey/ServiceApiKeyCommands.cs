using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Service.ApiKey
{
    internal class ServiceApiKeyCommands : ContainerCommandBase
    {
        public ServiceApiKeyCommands(IQuilt4NetClient client)
            : base("ApiKey")
        {
            RegisterCommand(new ServiceApiKeyGetCommand(client));
            RegisterCommand(new ServiceApiKeySetCommand(client));
        }
    }
}