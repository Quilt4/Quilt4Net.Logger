using System.Linq;
using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Session
{
    internal class ListSessionsCommand : ActionCommandBase
    {
        private readonly Client _client;

        public ListSessionsCommand(Client client)
            : base("List", "List sessions")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            return _client.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var sessions = (await _client.Session.GetListAsync()).ToArray();
            if (!sessions.Any()) return true;
            var title = new[] { new[] { "SessionKey", "Environment" } };
            var data = title.Union(sessions.Select(x => new[] { x.SessionKey.ToString(), x.Environment }).ToArray()).ToArray();
            OutputTable(data);
            return true;
        }
    }
}