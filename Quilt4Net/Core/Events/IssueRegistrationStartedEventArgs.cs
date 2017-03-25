using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class IssueRegistrationStartedEventArgs : EventArgs
    {
        internal IssueRegistrationStartedEventArgs(IssueRequest request)
        {
            Request = request;
        }

        public IssueRequest Request { get; }
    }
}