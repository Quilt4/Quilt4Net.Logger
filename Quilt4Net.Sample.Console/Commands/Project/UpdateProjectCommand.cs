using System.Linq;
using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Project
{
    internal class UpdateProjectCommand : ActionCommandBase
    {
        private readonly Client _client;

        public UpdateProjectCommand(Client client)
            : base("Update", "Update a project")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            return _client.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var projectKey = QueryParam("Project", GetParam(paramList, index++), (await _client.Project.GetListAsync()).ToDictionary(x => x.ProjectKey, x => x.Name));
            var projectName = QueryParam<string>("Name", GetParam(paramList, index++) );
            var dashboardColor = QueryParam<string>("Color", GetParam(paramList, index++));
            await _client.Project.UpdateAsync(projectKey, projectName, dashboardColor);
            return true;
        }
    }
}