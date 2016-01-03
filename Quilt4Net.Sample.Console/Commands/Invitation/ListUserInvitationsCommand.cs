using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Invitation
{
    internal class ListUserInvitationsCommand : ActionCommandBase
    {
        private readonly IClient _client;

        public ListUserInvitationsCommand(IClient client)
            : base("List", "List the users invitations")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var result = await _client.Action.Invitation.GetListAsync();
            foreach (var item in result)
            {
                OutputInformation("{0}\t{1}\t{2}\t{3}", item.ProjectName, item.InviteCode, item.InvitedByUserName, item.UserName ?? item.UserEMail);
            }

            return true;
        }
    }
}