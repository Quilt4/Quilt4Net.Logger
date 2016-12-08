using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.Service.Address
{
    internal class ServiceAddressGetCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public ServiceAddressGetCommand(IQuilt4NetClient client)
            : base("Get", "Get the current service address.")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            OutputInformation(_client.Configuration.Target.Location);
            return true;
        }
    }
}