using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.User
{
    internal class UserCommands : ContainerCommandBase
    {
        public UserCommands(IQuilt4NetClient client)
            : base("User")
        {
            RegisterCommand(new RegisterUserCommand(client));
            RegisterCommand(new LoginUserCommand(client));
            RegisterCommand(new LogoutUserCommand(client));
            //RegisterCommand(new GetUserInfoCommand(client));
            //RegisterCommand(new ChangeUserPasswordCommand(client));
            //RegisterCommand(new ListUsersCommand(client));
            //RegisterCommand(new SearchUsersCommand(client));
            //RegisterCommand(new UserRoleCommands(client));
        }
    }
}