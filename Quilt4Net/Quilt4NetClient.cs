using System;
using Quilt4Net.Core.Interfaces;
using Quilt4Net.Core.Lookups;
using Quilt4Net.Core.WebApi;
using Quilt4Net.Handlers;
using Quilt4Net.Lookups;
using Action = Quilt4Net.Core.Actions.Action;

namespace Quilt4Net
{
    public class Quilt4NetClient : IQuilt4NetClient
    {
        //private static readonly object _syncRoot = new object();
        //private static IClient _instance;
        //private static bool _instanceCreated = false;

        private readonly IConfigurationHandler _configurationHandler;
        private readonly IWebApiClient _webApiClient;
        private readonly Lazy<IIssueHandler> _issue;
        private readonly Lazy<ISessionHandler> _session;
        private readonly Lazy<IActions> _action;
        private readonly ILookup _lookup;

        public Quilt4NetClient(IConfigurationHandler configurationHandler)
        {
            //lock (_syncRoot)
            //{
            //    if (_instance != null) throw new InvalidOperationException("The client has been activated in singleton mode. Do not use 'Client.Instance' if you want to create your own instances of the client object.");

            var hashHandler = new HashHandler();
            _configurationHandler = configurationHandler;
            _lookup = new Lookup(new ApplicationLookup(_configurationHandler, hashHandler), new MachineLookup(hashHandler), new UserLookup(hashHandler));
            _webApiClient = new WebApiClient(_configurationHandler);
            _issue = new Lazy<IIssueHandler>(() => new IssueHandler(_session, _webApiClient, _configurationHandler));
            _session = new Lazy<ISessionHandler>(() => new SessionHandler(_webApiClient, _configurationHandler, Lookup.AplicationLookup, Lookup.MachineLookup, Lookup.UserLookup));
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

        public IConfigurationHandler ConfigurationHandler => _configurationHandler;
        public IWebApiClient WebApiClient => _webApiClient;
        public IIssueHandler IssueHandler => _issue.Value;
        public ISessionHandler SessionHandler => _session.Value;
        public IActions Actions => _action.Value;
        public ILookup Lookup => _lookup;

        public void Dispose()
        {
            if (_session.IsValueCreated)
            {
                _session.Value.End();
            }
        }
    }
}