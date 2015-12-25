using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Project
{
    internal class CreateProjectCommand : ActionCommandBase
    {
        private readonly IClient _client;

        public CreateProjectCommand(IClient client)
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