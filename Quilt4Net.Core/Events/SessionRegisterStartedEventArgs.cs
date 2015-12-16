using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class SessionRegisterStartedEventArgs : EventArgs
    {
        public SessionRegisterStartedEventArgs(SessionData request)
        {
            Request = request;
        }

        public SessionData Request { get; }
    }
}