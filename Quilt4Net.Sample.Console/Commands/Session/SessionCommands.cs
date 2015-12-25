using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Session
{
    internal class SessionCommands : ContainerCommandBase
    {
        public SessionCommands(IClient client)
            : base("Session")
        {
            RegisterCommand(new ListSessionsCommand(client));
            RegisterCommand(new RegisterSessionCommand(client));
            RegisterCommand(new EndSessionCommand(client));
        }
    }
}