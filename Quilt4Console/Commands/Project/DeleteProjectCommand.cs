using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.Project
{
    internal class DeleteProjectCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public DeleteProjectCommand(IQuilt4NetClient client)
            : base("Delete", "Delete a project")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            return _client.Actions.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var projectKey = QueryParam("Project", GetParam(paramList, index++), (await _client.Actions.Project.GetListAsync()).ToDictionary(x => x.ProjectKey, x => x.Name));
            await _client.Actions.Project.DeleteAsync(projectKey);
            return true;
        }
    }
}