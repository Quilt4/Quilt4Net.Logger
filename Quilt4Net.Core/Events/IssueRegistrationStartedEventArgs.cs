using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class IssueRegistrationStartedEventArgs : EventArgs
    {
        public IssueRegistrationStartedEventArgs(IssueRequest request)
        {
            Request = request;
        }

        public IssueRequest Request { get; }
    }
}