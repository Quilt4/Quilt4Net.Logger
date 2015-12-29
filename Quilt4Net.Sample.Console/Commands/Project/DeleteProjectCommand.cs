using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Project
{
    internal class DeleteProjectCommand : ActionCommandBase
    {
        private readonly IClient _client;

        public DeleteProjectCommand(IClient client)
            : base("Delete", "Delete a project")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            return _client.Action.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var projectKey = QueryParam("Project", GetParam(paramList, index++), (await _client.Action.Project.GetListAsync()).ToDictionary(x => x.ProjectKey,x => x.Name));
            await _client.Action.Project.DeleteAsync(projectKey);
            return true;
        }
    }
}