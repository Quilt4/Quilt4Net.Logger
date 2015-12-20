using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class IssueRegistrationStartedEventArgs : EventArgs
    {
        public IssueRegistrationStartedEventArgs(IssueData data)
        {
            Data = data;
        }

        public IssueData Data { get; }
    }
}