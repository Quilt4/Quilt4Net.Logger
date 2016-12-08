using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.Project
{
    internal class ListProjectsCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public ListProjectsCommand(IQuilt4NetClient client)
            : base("List", "List projects.")
        {
            _client = client;
        }

        public override bool CanExecute(out string reasonMessage)
        {
            reasonMessage = "Not logged on.";
            return _client.Actions.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var projects = (await _client.Actions.Project.GetListAsync()).ToArray();
            if (!projects.Any()) return true;
            var title = new[] { new[] { "Name", "ProjectApiKey" } };
            var data = title.Union(projects.Select(x => new[] { x.Name, x.ProjectApiKey }).ToArray()).ToArray();
            OutputTable(data);
            return true;
        }
    }
}