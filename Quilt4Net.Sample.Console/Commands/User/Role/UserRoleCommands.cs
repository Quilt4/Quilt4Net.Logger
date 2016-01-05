using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.User.Role
{
    internal class UserRoleCommands : ContainerCommandBase
    {
        public UserRoleCommands(IQuilt4NetClient client)
            : base("Role")
        {
            RegisterCommand(new UserRoleAddCommand(client));
        }
    }
}