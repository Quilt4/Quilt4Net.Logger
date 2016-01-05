using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Invitation
{
    internal class CreateInvitationCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public CreateInvitationCommand(IQuilt4NetClient client)
            : base("Create", "Create a new invitation.")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var projectKey = QueryParam("Project", GetParam(paramList, index++), (await _client.Actions.Project.GetListAsync()).ToDictionary(x => x.ProjectKey, x => x.Name));
            var user = QueryParam<string>("UserName/EMail", GetParam(paramList, index++));

            await _client.Actions.Invitation.CreateAsync(projectKey, user);

            return true;
        }
    }
}