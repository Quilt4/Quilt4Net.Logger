using System.Linq;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.Invitation
{
    internal class AcceptInvitationCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public AcceptInvitationCommand(IQuilt4NetClient client)
            : base("Accept", "Accept an invitation")
        {
            _client = client;
        }

        public override void Invoke(string[] param)
        {
            var projectInviteCode = QueryParam("Project", param, _client.Actions.Invitation.GetListAsync().Result.ToDictionary(x => x.InviteCode, x => x.ProjectName));
            _client.Actions.Invitation.Accept(projectInviteCode);
        }
    }
}