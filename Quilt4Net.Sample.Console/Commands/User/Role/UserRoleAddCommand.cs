using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

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

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var username = QueryParam<string>("UserName", GetParam(paramList, index++));
            var roleName = QueryParam<string>("RoleName", GetParam(paramList, index++));

            await _client.Actions.User.AddRoleAsync(username, roleName);
            return true;
        }
    }
}