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
            var databaseInfo = response.Database;
            OutputInformation("Database: {0}", databaseInfo.CanConnect ? $"Database {databaseInfo.Database}, Patch version {databaseInfo.Version}." : "Cannot connect to database.");
            OutputInformation("HasOwnProjectApiKey: {0}", response.HasOwnProjectApiKey);
            OutputInformation("CanWriteToSystemLog: {0}", response.CanWriteToSystemLog);
            OutputInformation("StartTime: {0}", response.StartTime);

            return true;
        }
    }
}