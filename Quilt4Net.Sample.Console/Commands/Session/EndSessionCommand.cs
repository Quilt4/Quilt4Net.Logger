using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Session
{
    internal class EndSessionCommand : ActionCommandBase
    {
        private readonly Client _client;

        public EndSessionCommand(Client client)
            : base("End", "End session.")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            return _client.Session.IsRegistered;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            await _client.Session.EndAsync();
            return true;
        }
    }
}