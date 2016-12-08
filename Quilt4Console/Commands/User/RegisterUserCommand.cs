using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.User
{
    internal class RegisterUserCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public RegisterUserCommand(IQuilt4NetClient client)
            : base("Register", "Register a new user")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var userName = QueryParam<string>("UserName", GetParam(paramList, index++));
            var email = QueryParam<string>("EMail", GetParam(paramList, index++));
            var fullName = QueryParam<string>("Full Name", GetParam(paramList, index++));
            var password = QueryParam<string>("Password", GetParam(paramList, index++));
            await _client.Actions.User.CreateAsync(userName, email, fullName, password);
            return true;
        }
    }
}