using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

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

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var projectInviteCode = QueryParam("Project", GetParam(paramList, index++), (await _client.Actions.Invitation.GetListAsync()).ToDictionary(x => x.InviteCode, x => x.ProjectName));
            await _client.Actions.Invitation.Accept(projectInviteCode);

            return true;
        }
    }
}