using System;

namespace Quilt4Net.Core.Events
{
    public class SessionEndStartedEventArgs : EventArgs
    {
        public SessionEndStartedEventArgs(string sessionToken)
        {
            SessionToken = sessionToken;
        }

        public string SessionToken { get; }
    }
}