using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class CreateUserCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public CreateUserCommand(IQuilt4NetClient client)
            : base("Create", "Create a new user")
        {
            _client = client;
        }

        public override void Invoke(string[] param)
        {
            var userName = QueryParam<string>("UserName", param);
            var email = QueryParam<string>("EMail", param);
            var firstName = QueryParam<string>("First Name", param);
            var lastName = QueryParam<string>("Last Name", param);
            var password = QueryParam<string>("Password", param);
            _client.Actions.User.CreateAsync(userName, email, firstName, lastName, password);
        }
    }
}