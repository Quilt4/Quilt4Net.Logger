using System;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.Session
{
    internal class ListSessionsCommand : ActionCommandBase
    {
        private readonly ISessionHandler _sessionHandler;

        public ListSessionsCommand(ISessionHandler sessionHandler)
            : base("List", "List sessions")
        {
            _sessionHandler = sessionHandler;
        }

        public override bool CanExecute(out string reasonMessage)
        {
            reasonMessage = string.Empty;
            if (!_sessionHandler.Client.Actions.User.IsAuthorized)
                reasonMessage = "Not Authorized";
            return _sessionHandler.Client.Actions.User.IsAuthorized;
        }

        public override void Invoke(string[] param)
        {
            throw new NotImplementedException();
            //var sessions = (await _sessionHandler.GetListAsync()).ToArray();
            //if (!sessions.Any()) return true;
            //var title = new[] { new[] { "SessionKey", "Environment" } };
            //var data = title.Union(sessions.Select(x => new[] { x.SessionKey.ToString(), x.Environment }).ToArray()).ToArray();
            //OutputTable(data);
            //return true;
        }
    }
}