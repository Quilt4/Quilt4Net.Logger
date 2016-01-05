using System;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;
using Action = Quilt4Net.Core.Actions.Action;

namespace Quilt4Net
{
    public class Quilt4NetClient : IQuilt4NetClient
    {
        private readonly IConfiguration _configuration;
        private readonly IWebApiClient _webApiClient;
        private readonly Lazy<IIssueHandler> _issue;
        private readonly Lazy<ISessionHandler> _session;
        private readonly Lazy<IActions> _action;
        private readonly IInformation _information;

        public Quilt4NetClient(IConfiguration configuration)
        {
            var hashHandler = new HashHandler();
            _configuration = configuration;
            _information = new Information(new ApplicationInformation(_configuration, hashHandler), new MachineInformation(hashHandler), new UserInformation(hashHandler));
            _webApiClient = new WebApiClient(_configuration);
            _issue = new Lazy<IIssueHandler>(() => new IssueHandler(_session, _webApiClient, _configuration));
            _session = new Lazy<ISessionHandler>(() => new SessionHandler(_webApiClient, _configuration, Information.AplicationInformation, Information.MachineInformation, Information.UserInformation));
            _action = new Lazy<IActions>(() => new Action(_webApiClient));
        }

        public IConfiguration Configuration => _configuration;
        public IWebApiClient WebApiClient => _webApiClient;
        public IIssueHandler Issue => _issue.Value;
        public ISessionHandler Session => _session.Value;
        public IActions Actions => _action.Value;
        public IInformation Information => _information;

        public void Dispose()
        {
            if (_session.IsValueCreated)
            {
                _session.Value.End();
            }
        }
    }
}