using System;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class Client : IClient
    {
        private static readonly object _syncRoot = new object();
        private static IClient _instance;
        private static bool _instanceCreated = false;

        private readonly IConfiguration _configuration;
        private readonly IWebApiClient _webApiClient;
        //private readonly Lazy<IAction> _action;
        private readonly Lazy<IUser> _user;
        private readonly Lazy<IProject> _project;
        private readonly Lazy<IApplication> _application;
        private readonly Lazy<IVersion> _version;
        private readonly Lazy<ISession> _session;
        private readonly Lazy<IIssue> _issue;
        private readonly Lazy<IService> _service;

        public Client(IConfiguration configuration)
        {
            lock (_syncRoot)
            {
                if (_instance != null) throw new InvalidOperationException("The client has been activated in singleton mode. Do not use 'Client.Instance' if you want to create your own instances of the client object.");

                _configuration = configuration;
                _webApiClient = new WebApiClient(_configuration);
                //_action = new Lazy<IAction>(() => new Core.Action(_webApiClient, _configuration));
                _user = new Lazy<IUser>(() => new User(_webApiClient));
                _project = new Lazy<IProject>(() => new Project(_webApiClient));
                _application = new Lazy<IApplication>(() => new Application(_webApiClient));
                _version = new Lazy<IVersion>(() => new Core.Version(_webApiClient));
                _session = new Lazy<ISession>(() => new Session(_webApiClient, _configuration, new ApplicationHelper(_configuration), new MachineHelper(), new UserHelper()));
                _issue = new Lazy<IIssue>(() => new Issue(_session, _webApiClient, _configuration));
                _service = new Lazy<IService>(() => new Service(_webApiClient));

                _instanceCreated = true;
            }
        }

        public static IClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            if (_instanceCreated) throw new InvalidOperationException("The client has been activated in instance mode. Do not use 'new Client(???)' if you want to use the singleton instance of the client object.");

                            _instance = new Client(Quilt4Net.Configuration.Instance);
                        }
                    }
                }

                return _instance;
            }
        }

        public IConfiguration Configuration => _configuration;
        public IWebApiClient WebApiClient => _webApiClient;
        //public IAction Action => _action.Value;
        public IUser User => _user.Value;
        public IProject Project => _project.Value;
        public IApplication Application => _application.Value;
        public IVersion Version => _version.Value;
        public ISession Session => _session.Value;
        public IIssue Issue => _issue.Value;
        public IService Service => _service.Value;

        public void Dispose()
        {
            if (_session.IsValueCreated)
            {
                _session.Value.End();
            }
        }
    }
}