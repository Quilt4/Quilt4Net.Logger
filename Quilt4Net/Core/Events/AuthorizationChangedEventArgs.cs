using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class AuthorizationChangedEventArgs : EventArgs
    {
        private readonly Authorization _authorization;

        internal AuthorizationChangedEventArgs(string userName, Authorization authorization)
        {
            UserName = userName;
            _authorization = authorization;
        }

        public string UserName { get; }
        public bool IsAuthorized => _authorization != null;
    }
}