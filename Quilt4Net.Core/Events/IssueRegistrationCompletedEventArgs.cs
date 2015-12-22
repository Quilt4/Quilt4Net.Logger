using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class IssueRegistrationCompletedEventArgs : EventArgs
    {
        public IssueRegistrationCompletedEventArgs(IssueRequest request, IssueResult result)
        {
            Request = request;
            Result = result;
        }

        public IssueRequest Request { get; }
        public IssueResult Result { get; }
    }
}