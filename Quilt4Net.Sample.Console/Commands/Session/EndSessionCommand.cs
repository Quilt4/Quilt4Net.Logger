using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Session
{
    internal class EndSessionCommand : ActionCommandBase
    {
        private readonly ISessionHandler _sessionHandler;

        public EndSessionCommand(ISessionHandler sessionHandler)
            : base("End", "End session.")
        {
            _sessionHandler = sessionHandler;
        }

        public override bool CanExecute()
        {
            return _sessionHandler.IsRegisteredOnServer;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            await _sessionHandler.EndAsync();
            return true;
        }
    }
}