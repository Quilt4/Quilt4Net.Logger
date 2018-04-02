using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

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

        public override bool CanExecute(out string reasonMessage)
        {
            reasonMessage = string.Empty;
            if (!_sessionHandler.IsRegisteredOnServer)
                reasonMessage = "Not registered on server";
            return _sessionHandler.IsRegisteredOnServer;
        }

        public override void Invoke(string[] param)
        {
            _sessionHandler.EndAsync();
        }
    }
}