using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class SessionRegistrationCompletedEventArgs : EventArgs
    {
        public SessionRegistrationCompletedEventArgs(SessionRequest request, SessionResponse response)
        {
            Request = request;
            Response = response;
        }

        public SessionRequest Request { get; }
        public SessionResponse Response { get; }
    }
}