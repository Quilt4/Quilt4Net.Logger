﻿using System;
using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.Session
{
    internal class ListSessionsCommand : ActionCommandBase
    {
        private readonly Quilt4Net.Core.Interfaces.ISessionHandler _sessionHandler;

        public ListSessionsCommand(Quilt4Net.Core.Interfaces.ISessionHandler sessionHandler)
            : base("List", "List sessions")
        {
            _sessionHandler = sessionHandler;
        }

        public override bool CanExecute()
        {
            return _sessionHandler.Client.Actions.User.IsAuthorized;
        }

        public override async Task<bool> InvokeAsync(string paramList)
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