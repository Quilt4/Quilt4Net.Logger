using Quilt4Net.Core.Interfaces;
using Quilt4Net.Sample.Console.Commands.User.Role;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class UserCommands : ContainerCommandBase
    {
        public UserCommands(IClient client)
            : base("User")
        {
            RegisterCommand(new CreateUserCommand(client));
            RegisterCommand(new LoginUserCommand(client));
            RegisterCommand(new LogoutUserCommand(client));
            RegisterCommand(new GetUserInfoCommand(client));
            RegisterCommand(new ChangeUserPasswordCommand(client));
            RegisterCommand(new ListUsersCommand(client));
            RegisterCommand(new UserRoleCommands(client));
        }
    }
}