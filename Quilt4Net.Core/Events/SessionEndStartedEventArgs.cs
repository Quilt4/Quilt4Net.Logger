using System;

namespace Quilt4Net.Core.Events
{
    public class SessionEndStartedEventArgs : EventArgs
    {
        public SessionEndStartedEventArgs(Guid sessionKey)
        {
            SessionKey = sessionKey;
        }

        public Guid SessionKey { get; }
    }
}