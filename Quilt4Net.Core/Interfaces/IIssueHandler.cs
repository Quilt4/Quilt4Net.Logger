using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;
using Quilt4Net.Core.Handlers;

namespace Quilt4Net.Core.Interfaces
{
    public interface IIssueHandler
    {
        event EventHandler<IssueRegistrationStartedEventArgs> IssueRegistrationStartedEvent;
        event EventHandler<IssueRegistrationCompletedEventArgs> IssueRegistrationCompletedEvent;
        Task<IssueResult> RegisterAsync(string message, IssueHandlerBase.MessageIssueLevel issueLevel, string userHandle = null, IDictionary<string, string> data = null);
        void RegisterStart(string message, IssueHandlerBase.MessageIssueLevel issueLevel, string userHandle = null, IDictionary<string, string> data = null);
        IssueResult Register(string message, IssueHandlerBase.MessageIssueLevel issueLevel, string userHandle = null, IDictionary<string, string> data = null);
        Task<IssueResult> RegisterAsync(Exception exception, IssueHandlerBase.ExceptionIssueLevel issueLevel = IssueHandlerBase.ExceptionIssueLevel.Error, string userHandle = null);
        void RegisterStart(Exception exception, IssueHandlerBase.ExceptionIssueLevel issueLevel = IssueHandlerBase.ExceptionIssueLevel.Error, string userHandle = null);
        IssueResult Register(Exception exception, IssueHandlerBase.ExceptionIssueLevel issueLevel = IssueHandlerBase.ExceptionIssueLevel.Error, string userHandle = null);
        Task<IEnumerable<IssueTypeResponse>> GetIssueTypesAsync(Guid versionKey);
    }
}