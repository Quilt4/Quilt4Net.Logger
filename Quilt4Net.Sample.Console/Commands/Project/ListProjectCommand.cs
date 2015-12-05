using System.Linq;
using System.Threading.Tasks;
using Tharga.Quilt4Net;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Project
{
    internal class ListProjectCommand : ActionCommandBase
    {
        private readonly Client _client;

        public ListProjectCommand(Client client)
            : base("List", "List projects")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            return _client.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var projects = await _client.Project.GetListAsync();
            var title = new[] { new[] { "Name", "ProjectApiKey" } };
            var data = title.Union(projects.Select(x => new[] { x.Name, x.ProjectApiKey }).ToArray()).ToArray();
            OutputTable(data);
            return true;
        }
    }
}