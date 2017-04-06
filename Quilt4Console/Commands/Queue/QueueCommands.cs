using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.Queue
{
    internal class QueueCommands : ContainerCommandBase
    {
        public QueueCommands(IClient client)
            : base("Queue")
        {
            RegisterCommand(new ListQueueCommand(client));
        }
    }

    internal class ListQueueCommand : ActionCommandBase
    {
        private readonly IClient _client;

        public ListQueueCommand(IClient client)
            : base("List", "List queued commands.")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var list = _client.GetAll();
            var table = list.Select(x => new[] { "A", "B" }).ToArray();
            OutputTable(table);

            return true;
        }
    }
}
