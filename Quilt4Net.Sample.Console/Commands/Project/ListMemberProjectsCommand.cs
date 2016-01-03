using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Project
{
    internal class ListMemberProjectsCommand : ActionCommandBase
    {
        private readonly IClient _client;

        public ListMemberProjectsCommand(IClient client)
            : base("Members", "List all members for a project.")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var projectKey = QueryParam("Project", GetParam(paramList, index++), (await _client.Action.Project.GetListAsync()).ToDictionary(x => x.ProjectKey, x => x.Name));
            var members = await _client.Action.Project.GetMembersAsync(projectKey);
            foreach (var member in members)
            {
                OutputInformation("{0}\t{1}\t{2}\t{3}", member.UserName, member.EMail, member.Confirmed, member.Role);
            }

            return true;
        }
    }
}