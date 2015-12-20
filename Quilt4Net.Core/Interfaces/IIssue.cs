using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;

namespace Quilt4Net.Core.Interfaces
{
    public interface IIssue
    {
        event EventHandler<IssueRegisterStartedEventArgs> IssueRegisteredStartedEvent;
        event EventHandler<IssueRegisterCompletedEventArgs> IssueRegisteredCompletedEvent;
        Task<IssueResponse> RegisterAsync(string message, Issue.MessageIssueLevel issueLevel, string userHandle = null, IDictionary<string, string> data = null);
        void RegisterStart(string message, Issue.MessageIssueLevel issueLevel, string userHandle = null, IDictionary<string, string> data = null);
        IssueResponse Register(string message, Issue.MessageIssueLevel issueLevel, string userHandle = null, IDictionary<string, string> data = null);
        Task<IssueResponse> RegisterAsync(Exception exception, Issue.ExceptionIssueLevel issueLevel = Issue.ExceptionIssueLevel.Error, string userHandle = null);
        void RegisterStart(Exception exception, Issue.ExceptionIssueLevel issueLevel = Issue.ExceptionIssueLevel.Error, string userHandle = null);
        IssueResponse Register(Exception exception, Issue.ExceptionIssueLevel issueLevel = Issue.ExceptionIssueLevel.Error, string userHandle = null);
    }
}