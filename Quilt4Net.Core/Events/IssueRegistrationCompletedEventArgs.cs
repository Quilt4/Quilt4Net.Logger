using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class IssueRegistrationCompletedEventArgs : EventArgs
    {
        public IssueRegistrationCompletedEventArgs(IssueData data, IssueResult result)
        {
            Data = data;
            Result = result;
        }

        public IssueData Data { get; }
        public IssueResult Result { get; }
    }
}