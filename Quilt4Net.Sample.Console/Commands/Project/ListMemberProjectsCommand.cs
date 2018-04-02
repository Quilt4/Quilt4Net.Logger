using System.Linq;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

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

        public override void Invoke(string[] param)
        {
            var projectKey = QueryParam("Project", param, _client.Actions.Project.GetListAsync().Result.ToDictionary(x => x.ProjectKey, x => x.Name));
            var members = _client.Actions.Project.GetMembersAsync(projectKey).Result;
            foreach (var member in members) OutputInformation($"{member.UserName}\t{member.EMail}\t{member.Confirmed}\t{member.Role}");
        }
    }
}