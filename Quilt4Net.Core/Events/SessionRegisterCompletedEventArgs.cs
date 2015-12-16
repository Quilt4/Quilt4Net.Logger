using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class SessionRegisterCompletedEventArgs : EventArgs
    {
        public SessionRegisterCompletedEventArgs(SessionData request, SessionResponse response)
        {
            Request = request;
            Response = response;
        }

        public SessionData Request { get; }
        public SessionResponse Response { get; }
    }
}