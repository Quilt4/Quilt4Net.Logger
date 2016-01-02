using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Invitation
{
    internal class ListUserInvitationsCommand : ActionCommandBase
    {
        private readonly IClient _client;

        public ListUserInvitationsCommand(IClient client)
            : base("Invitations", "List")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            //TODO: List all invitations for current user
            throw new System.NotImplementedException();
        }
    }
}