using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Service.Address
{
    internal class ServiceAddressSetCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public ServiceAddressSetCommand(IQuilt4NetClient client)
            : base("Set", "Set the current service address.")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var address = QueryParam<string>("Address", GetParam(paramList, index++), new Dictionary<string, string> { { _client.Configuration.Target.Location , _client.Configuration.Target.Location } });
            _client.Configuration.Target.Location = address;

            return true;
        }
    }
}