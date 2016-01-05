using System;
using Quilt4Net.Core.Informations;
using Quilt4Net.Core.Interfaces;
using Quilt4Net.Core.WebApi;
using Action = Quilt4Net.Core.Actions.Action;

namespace Quilt4Net
{
    public class Quilt4NetClient : IQuilt4NetClient
    {
        //private static readonly object _syncRoot = new object();
        //private static IClient _instance;
        //private static bool _instanceCreated = false;

        private readonly IConfiguration _configuration;
        private readonly IWebApiClient _webApiClient;
        private readonly Lazy<IIssueHandler> _issue;
        private readonly Lazy<ISessionHandler> _session;
        private readonly Lazy<IActions> _action;
        private readonly IInformation _information;

        public Quilt4NetClient(IConfiguration configuration)
        {
            //lock (_syncRoot)
            //{
            //    if (_instance != null) throw new InvalidOperationException("The client has been activated in singleton mode. Do not use 'Client.Instance' if you want to create your own instances of the client object.");

            var hashHandler = new HashHandler();
            _configuration = configuration;
            _information = new Information(new ApplicationInformation(_configuration, hashHandler), new MachineInformation(hashHandler), new UserInformation(hashHandler));
            _webApiClient = new WebApiClient(_configuration);
            _issue = new Lazy<IIssueHandler>(() => new IssueHandler(_session, _webApiClient, _configuration));
            _session = new Lazy<ISessionHandler>(() => new SessionHandler(_webApiClient, _configuration, Information.AplicationInformation, Information.MachineInformation, Information.UserInformation));
            _action = new Lazy<IActions>(() => new Action(_webApiClient));

            //    _instanceCreated = true;
            //}
        }

        //public static IClient Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            lock (_syncRoot)
        //            {
        //                if (_instance == null)
        //                {
        //                    if (_instanceCreated) throw new InvalidOperationException("The client has been activated in instance mode. Do not use 'new Client(???)' if you want to use the singleton instance of the client object.");

        //                    _instance = new Client(Quilt4Net.Configuration.Instance);
        //                }
        //            }
        //        }

        //        return _instance;
        //    }
        //}

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