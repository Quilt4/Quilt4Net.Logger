using System;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;

namespace Quilt4Net.Core.Interfaces
{
    public interface IIssue
    {
        event EventHandler<IssueRegisterStartedEventArgs> IssueRegisteredStartedEvent;
        event EventHandler<IssueRegisterCompletedEventArgs> IssueRegisteredCompletedEvent;
        Task<IssueResponse> RegisterAsync();
        void RegisterStart();
        IssueResponse Register();
    }
}