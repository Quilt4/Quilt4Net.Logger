using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Project
{
    internal class CreateProjectCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public CreateProjectCommand(IQuilt4NetClient client)
            : base("Create", "Create a new project")
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
            var projectName = QueryParam<string>("Name", GetParam(paramList, index++));
            await _client.Actions.Project.CreateAsync(projectName);
            return true;
        }
    }
}