using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Project
{
    internal class ListMemberProjectsCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public ListMemberProjectsCommand(IQuilt4NetClient client)
            : base("Members", "List all members for a project.")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var projectKey = QueryParam("Project", GetParam(paramList, index++), (await _client.Actions.Project.GetListAsync()).ToDictionary(x => x.ProjectKey, x => x.Name));
            var members = await _client.Actions.Project.GetMembersAsync(projectKey);
            foreach (var member in members)
            {
                OutputInformation("{0}\t{1}\t{2}\t{3}", member.UserName, member.EMail, member.Confirmed, member.Role);
            }

            return true;
        }
    }
}