using System;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class Client
    {
        private readonly IConfiguration _configuration;
        private readonly IWebApiClient _webApiClient;
        private readonly Lazy<User> _user;
        private readonly Lazy<Project> _project;
        private readonly Lazy<ISession> _session;
        private readonly Lazy<Issue> _issue;

        public Client()
            : this(new Configuration())
        {
        }

        private Client(IConfiguration configuration)
        {
            _configuration = configuration;
            _webApiClient = new WebApiClient(_configuration);
            _user = new Lazy<User>(() => new User(_webApiClient));
            _project = new Lazy<Project>(() => new Project(_webApiClient));
            _session = new Lazy<ISession>(() => new Session(_webApiClient, _configuration, new ApplicationHelper(_configuration), new MachineHelper(), new UserHelper()));
            _issue = new Lazy<Issue>(() => new Issue(_webApiClient));
        }

        public IConfiguration Configuration => _configuration;
        public IWebApiClient WebApiClient => _webApiClient;
        public User User => _user.Value;
        public Project Project => _project.Value;
        public ISession Session => _session.Value;
        public Issue Issue => _issue.Value;
    }
}