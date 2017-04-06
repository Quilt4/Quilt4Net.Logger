using System;
using System.Threading.Tasks;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Service
{
    internal class ServicePostCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public ServicePostCommand(IQuilt4NetClient client)
            : base("Post", "Post raw json data")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            throw new NotImplementedException();
            //var index = 0;
            //var controller = QueryParam<string>("Controller", GetParam(paramList, index++));
            //var jsonData = QueryParam<string>("Json Data", GetParam(paramList, index++));

            //var basic = new BasicDataHandler(_client.Client);
            //var result = await basic.Post(controller, jsonData);

            //OutputInformation(result);

            //return true;
        }
    }
}