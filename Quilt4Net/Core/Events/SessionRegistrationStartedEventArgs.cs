using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class SessionRegistrationStartedEventArgs : EventArgs
    {
        internal SessionRegistrationStartedEventArgs(SessionRequest request)
        {
            Request = request;
        }

        public SessionRequest Request { get; }
    }
}