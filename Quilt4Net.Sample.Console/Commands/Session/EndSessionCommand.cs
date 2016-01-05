using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Session
{
    internal class EndSessionCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public EndSessionCommand(IQuilt4NetClient client)
            : base("End", "End session.")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            return _client.SessionHandler.IsRegistered;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            await _client.SessionHandler.EndAsync();
            return true;
        }
    }
}