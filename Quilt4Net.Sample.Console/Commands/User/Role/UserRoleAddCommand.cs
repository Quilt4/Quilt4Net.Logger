using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.User.Role
{
    internal class UserRoleAddCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public UserRoleAddCommand(IQuilt4NetClient client)
            : base("Add", "Add a role to a user.")
        {
            _client = client;
        }

        public override void Invoke(string[] param)
        {
            var username = QueryParam<string>("UserName", param);
            var roleName = QueryParam<string>("RoleName", param);

            _client.Actions.User.AddRoleAsync(username, roleName);
        }
    }
}