using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class SessionEndCompletedEventArgs : EventArgs
    {
        public SessionEndCompletedEventArgs(Guid sessionKey, EndSesionResult result)
        {
            SessionKey = sessionKey;
            Result = result;
        }

        public Guid SessionKey { get; }
        public EndSesionResult Result { get; }
    }
}