using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

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

        public override bool CanExecute()
        {
            return _client.Actions.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var projectKey = QueryParam("Project", GetParam(paramList, 0), (await _client.Actions.Project.GetListAsync()).ToDictionary(x => x.ProjectKey, x => x.Name));
            var project = await _client.Actions.Project.GetAsync(projectKey);
            OutputInformation("{0}\t{1}", project.Name, project.ProjectApiKey);
            return true;
        }
    }
}