using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class InviteUserCommand : ActionCommandBase
    {
        private readonly IClient _client;

        public InviteUserCommand(IClient client)
            : base("Invite", "Invite user to a project.")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var projectKey = QueryParam("Project", GetParam(paramList, index++), (await _client.Action.Project.GetListAsync()).ToDictionary(x => x.ProjectKey, x => x.Name));
            var user = QueryParam<string>("UserName/EMail", GetParam(paramList, index++));

            await _client.Action.User.InviteAsync(projectKey, user);

            return true;
        }
    }
}