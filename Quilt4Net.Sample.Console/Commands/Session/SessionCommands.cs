using Quilt4Net.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Session
{
    internal class SessionCommands : ContainerCommandBase
    {
        public SessionCommands(ISessionHandler sessionHandler)
            : base("Session")
        {
            RegisterCommand(new ListSessionsCommand(sessionHandler));
            RegisterCommand(new RegisterSessionCommand(sessionHandler));
            RegisterCommand(new EndSessionCommand(sessionHandler));
        }
    }
}