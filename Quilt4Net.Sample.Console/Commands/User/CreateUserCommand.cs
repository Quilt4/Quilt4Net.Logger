using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class CreateUserCommand : ActionCommandBase
    {
        private readonly IClient _client;

        public CreateUserCommand(IClient client)
            : base("Create", "Create a new user")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var userName = QueryParam<string>("UserName", GetParam(paramList,index++));
            var email = QueryParam<string>("EMail", GetParam(paramList, index++));
            var password = QueryParam<string>("Password", GetParam(paramList, index++));
            await _client.Action.User.CreateAsync(userName, email, password);
            return true;
        }
    }
}