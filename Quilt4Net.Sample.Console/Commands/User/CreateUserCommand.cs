using System.Threading.Tasks;
using Tharga.Quilt4Net;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class CreateUserCommand : ActionCommandBase
    {
        private readonly Client _client;

        public CreateUserCommand(Client client)
            : base("Create", "Create a new user")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var email = QueryParam<string>("EMail", GetParam(paramList,index++));
            var password = QueryParam<string>("Password", GetParam(paramList, index++));
            await _client.User.CreateAsync(email, password);
            return true;
        }
    }
}