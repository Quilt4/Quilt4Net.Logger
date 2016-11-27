using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Service.ApiKey
{
    internal class ServiceApiKeyGetCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public ServiceApiKeyGetCommand(IQuilt4NetClient client)
            : base("Get", "Get the current Api Key.")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            OutputInformation(_client.Configuration.ProjectApiKey);

            return true;
        }
    }
}