using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class SessionRegistrationStartedEventArgs : EventArgs
    {
        public SessionRegistrationStartedEventArgs(SessionData data)
        {
            Data = data;
        }

        public SessionData Data { get; }
    }
}