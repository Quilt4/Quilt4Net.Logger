using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

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

        public override void Invoke(string[] param)
        {
            var response = _client.Actions.Service.GetServiceInfo().Result;

            var msg = string.IsNullOrEmpty(response.Environment) ? "" : $" ({response.Environment})";
            OutputInformation($"Version: {response.Version}{msg}");
            if (!string.IsNullOrEmpty(response.Message))
            {
                OutputWarning(response.Message);
            }

            var databaseInfo = response.Database;
            if (databaseInfo != null)
            {
                var db = databaseInfo.CanConnect ? $"Database {databaseInfo.Database}, Patch version {databaseInfo.Version}." : "Cannot connect to database.";
                OutputInformation($"Database: {db}" );
                OutputInformation($"HasOwnProjectApiKey: {response.HasOwnProjectApiKey}");
                OutputInformation($"CanWriteToSystemLog: {response.CanWriteToSystemLog}");
                OutputInformation($"StartTime: {response.StartTime}");
            }
        }
    }
}