using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Service
{
    internal class ServiceInfoCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public ServiceInfoCommand(IQuilt4NetClient client)
            : base("Info", "Get information about the service.")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var response = await _client.Actions.Service.GetServiceInfo();

            OutputInformation("Version: {0} ({1})", response.Version, response.Environment);
            OutputInformation("Database: {0}", response.DatabaseInfo);

            return true;
        }
    }
}