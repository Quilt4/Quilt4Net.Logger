using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class SessionRegistrationCompletedEventArgs : EventArgs
    {
        public SessionRegistrationCompletedEventArgs(SessionData data, SessionResult result)
        {
            Data = data;
            Result = result;
        }

        public SessionData Data { get; }
        public SessionResult Result { get; }
    }
}