using Tharga.Quilt4Net;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class UserCommands : ContainerCommandBase
    {
        public UserCommands(Client client)
            : base("User")
        {
            RegisterCommand(new CreateUserCommand(client));
            RegisterCommand(new LoginUserCommand(client));
        }
    }
}