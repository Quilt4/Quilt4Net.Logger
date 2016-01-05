using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Invitation
{
    internal class InvitationCommands : ContainerCommandBase
    {
        public InvitationCommands(IClient client)
            : base("Invitation")
        {
            RegisterCommand(new CreateInvitationCommand(client));
            RegisterCommand(new ListUserInvitationsCommand(client));
            RegisterCommand(new AcceptInvitationCommand(client));
        }
    }
}