using System.Threading.Tasks;
using Tharga.Quilt4Net;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Project
{
    internal class CreateProjectCommand : ActionCommandBase
    {
        private readonly Client _client;

        public CreateProjectCommand(Client client)
            : base("Create", "Create a new project")
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
            var projectName = QueryParam<string>("Name", GetParam(paramList, index++));
            await _client.Project.CreateAsync(projectName);
            return true;
        }
    }
}