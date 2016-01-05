using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class AuthorizationChangedEventArgs : EventArgs
    {
        private readonly Authorization _authorization;

        internal AuthorizationChangedEventArgs(Authorization authorization)
        {
            _authorization = authorization;
        }

        public bool IsAuthorized => _authorization != null;
    }
}