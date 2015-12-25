using Quilt4Net.Core.Interfaces;
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
        }
    }
}