using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

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

        public override bool CanExecute(out string reasonMessage)
        {
            reasonMessage = string.Empty;
            if (!_client.Actions.User.IsAuthorized)
                reasonMessage = "Not Authorized";
            return _client.Actions.User.IsAuthorized;
        }

        public override void Invoke(string[] param)
        {
            var projectName = QueryParam<string>("Name", param);
            _client.Actions.Project.CreateAsync(projectName);
        }
    }
}