using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class SessionEndCompletedEventArgs : EventArgs
    {
        internal SessionEndCompletedEventArgs(string sessionKey, EndSesionResult result)
        {
            SessionKey = sessionKey;
            Result = result;
        }

        public string SessionKey { get; }
        public EndSesionResult Result { get; }
    }
}