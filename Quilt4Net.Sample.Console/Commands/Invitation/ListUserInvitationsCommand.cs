using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.Invitation
{
    internal class ListUserInvitationsCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public ListUserInvitationsCommand(IQuilt4NetClient client)
            : base("List", "List the users invitations")
        {
            _client = client;
        }

        public override void Invoke(string[] param)
        {
            var result = _client.Actions.Invitation.GetListAsync().Result;
            foreach (var item in result) OutputInformation($"{item.ProjectName}\t{item.InviteCode}\t{item.InvitedByUserName}\t{item.UserName ?? item.UserEMail}");
        }
    }
}