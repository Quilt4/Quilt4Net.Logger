using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Service.ApiKey
{
    internal class ServiceApiKeySetCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public ServiceApiKeySetCommand(IQuilt4NetClient client)
            : base("Set", "Set the current Api Key.")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var projectApiKey = QueryParam<string>("Project Api Key", GetParam(paramList, index++), new Dictionary<string, string> { { _client.Configuration.ProjectApiKey, _client.Configuration.ProjectApiKey } });
            _client.Configuration.ProjectApiKey = projectApiKey;

            return true;
        }
    }
}