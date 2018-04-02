using System.Linq;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

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

        public override bool CanExecute(out string reasonMessage)
        {
            reasonMessage = string.Empty;
            if (!_client.Actions.User.IsAuthorized)
                reasonMessage = "Not Authorized";
            return _client.Actions.User.IsAuthorized;
        }

        public override void Invoke(string[] param)
        {
            var projectKey = QueryParam("Project", param, _client.Actions.Project.GetListAsync().Result.ToDictionary(x => x.ProjectKey, x => x.Name));
            var projectName = QueryParam<string>("Name", param);
            var dashboardColor = QueryParam<string>("Color", param);
            _client.Actions.Project.UpdateAsync(projectKey, projectName, dashboardColor);
        }
    }
}