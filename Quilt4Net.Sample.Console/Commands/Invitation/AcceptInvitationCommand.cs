using System.Linq;
using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Invitation
{
    internal class AcceptInvitationCommand : ActionCommandBase
    {
        private readonly Client _client;

        public AcceptInvitationCommand(Client client)
            : base("Accept", "Accept an invitation")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var projectInviteCode = QueryParam("Project", GetParam(paramList, index++), (await _client.Action.Invitation.GetListAsync()).ToDictionary(x => x.InviteCode, x => x.ProjectName));
            await _client.Action.Invitation.Accept(projectInviteCode);

            return true;
        }
    }
}