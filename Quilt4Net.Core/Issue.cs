using System;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public class Issue : IIssue
    {
        private readonly IWebApiClient _webApiClient;

        internal Issue(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public event EventHandler<IssueRegisterStartedEventArgs> IssueRegisteredStartedEvent;
        public event EventHandler<IssueRegisterCompletedEventArgs> IssueRegisteredCompletedEvent;

        public Task<IssueResponse> RegisterAsync()
        {
            throw new NotImplementedException();
        }

        public void RegisterStart()
        {
            throw new NotImplementedException();
        }

        public IssueResponse Register()
        {
            throw new NotImplementedException();
        }
    }
}