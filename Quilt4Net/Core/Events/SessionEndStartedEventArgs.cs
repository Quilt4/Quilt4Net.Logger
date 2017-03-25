using System;

namespace Quilt4Net.Core.Events
{
    public class SessionEndStartedEventArgs : EventArgs
    {
        internal SessionEndStartedEventArgs(string sessionKey)
        {
            SessionKey = sessionKey;
        }

        public string SessionKey { get; }
    }
}