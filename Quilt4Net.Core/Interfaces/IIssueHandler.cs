using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;

namespace Quilt4Net.Core.Interfaces
{
    public interface IIssueHandler
    {
        IQuilt4NetClient Client { get; }
        event EventHandler<IssueRegistrationStartedEventArgs> IssueRegistrationStartedEvent;
        event EventHandler<IssueRegistrationCompletedEventArgs> IssueRegistrationCompletedEvent;
        Task<IssueResult> RegisterAsync(string message, MessageIssueLevel issueLevel = MessageIssueLevel.Error, string userHandle = null, IDictionary<string, string> data = null);
        void RegisterStart(string message, MessageIssueLevel issueLevel = MessageIssueLevel.Error, string userHandle = null, IDictionary<string, string> data = null);
        IssueResult Register(string message, MessageIssueLevel issueLevel = MessageIssueLevel.Error, string userHandle = null, IDictionary<string, string> data = null);
        Task<IssueResult> RegisterAsync(Exception exception, ExceptionIssueLevel issueLevel = ExceptionIssueLevel.Error, string userHandle = null);
        void RegisterStart(Exception exception, ExceptionIssueLevel issueLevel = ExceptionIssueLevel.Error, string userHandle = null);
        IssueResult Register(Exception exception, ExceptionIssueLevel issueLevel = ExceptionIssueLevel.Error, string userHandle = null);
        Task<IEnumerable<IssueTypeResponse>> GetIssueTypesAsync(Guid versionKey);
    }
}