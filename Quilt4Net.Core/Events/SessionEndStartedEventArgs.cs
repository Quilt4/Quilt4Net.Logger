using System;

namespace Quilt4Net.Core.Events
{
    public class SessionEndStartedEventArgs : EventArgs
    {
        internal SessionEndStartedEventArgs(string sessionToken)
        {
            SessionToken = sessionToken;
        }

        public string SessionToken { get; }
    }
}