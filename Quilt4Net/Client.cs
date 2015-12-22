using System;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class Client
    {
        private static Client _instance;

        private readonly IConfiguration _configuration;
        private readonly IWebApiClient _webApiClient;
        private readonly Lazy<User> _user;
        private readonly Lazy<Project> _project;
        private readonly Lazy<ISession> _session;
        private readonly Lazy<IIssue> _issue;

        public Client(IConfiguration configuration)
        {
            _configuration = configuration;
            _webApiClient = new WebApiClient(_configuration);
            _user = new Lazy<User>(() => new User(_webApiClient));
            _project = new Lazy<Project>(() => new Project(_webApiClient));
            _session = new Lazy<ISession>(() => new Session(_webApiClient, _configuration, new ApplicationHelper(_configuration), new MachineHelper(), new UserHelper()));
            _issue = new Lazy<IIssue>(() => new Issue(_session, _webApiClient, _configuration));
        }

        public static Client Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Client(Quilt4Net.Configuration.Instance);
                }

                return _instance;
            }
        }

        public IConfiguration Configuration => _configuration;
        public IWebApiClient WebApiClient => _webApiClient;
        public User User => _user.Value;
        public Project Project => _project.Value;
        public ISession Session => _session.Value;
        public IIssue Issue => _issue.Value;
    }
}