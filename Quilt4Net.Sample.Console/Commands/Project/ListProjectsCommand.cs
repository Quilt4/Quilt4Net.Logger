using System.Linq;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.Project
{
    internal class ListProjectsCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public ListProjectsCommand(IQuilt4NetClient client)
            : base("List", "List projects")
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
            var projects = _client.Actions.Project.GetListAsync().Result.ToArray();
            if (!projects.Any()) return;
            var title = new[] { new[] { "Name", "ProjectApiKey" } };
            var data = title.Union(projects.Select(x => new[] { x.Name, x.ProjectApiKey }).ToArray()).ToArray();
            OutputTable(data);
        }
    }
}