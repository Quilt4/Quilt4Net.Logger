using System.Linq;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.Project
{
    internal class GetProjectCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public GetProjectCommand(IQuilt4NetClient client)
            : base("GetApplicationData", "GetApplicationData a project")
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
            var projectKey = QueryParam("Project", param, (_client.Actions.Project.GetListAsync().Result).ToDictionary(x => x.ProjectKey, x => x.Name));
            var project = _client.Actions.Project.GetAsync(projectKey).Result;
            OutputInformation($"{project.Name}\t{project.ProjectApiKey}");
        }
    }
}