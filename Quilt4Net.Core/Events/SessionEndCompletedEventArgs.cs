using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class SessionEndCompletedEventArgs : EventArgs
    {
        internal SessionEndCompletedEventArgs(string sessionToken, EndSesionResult result)
        {
            SessionToken = sessionToken;
            Result = result;
        }

        public string SessionToken { get; }
        public EndSesionResult Result { get; }
    }
}