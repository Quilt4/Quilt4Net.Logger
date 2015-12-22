using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class SessionRegistrationCompletedEventArgs : EventArgs
    {
        public SessionRegistrationCompletedEventArgs(SessionRequest request, SessionResult result)
        {
            Request = request;
            Result = result;
        }

        public SessionRequest Request { get; }
        public SessionResult Result { get; }
    }
}