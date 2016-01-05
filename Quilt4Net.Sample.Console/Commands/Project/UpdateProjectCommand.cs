using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Project
{
    internal class UpdateProjectCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public UpdateProjectCommand(IQuilt4NetClient client)
            : base("Update", "Update a project")
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
            var projectName = QueryParam<string>("Name", GetParam(paramList, index++) );
            var dashboardColor = QueryParam<string>("Color", GetParam(paramList, index++));
            await _client.Actions.Project.UpdateAsync(projectKey, projectName, dashboardColor);
            return true;
        }
    }
}