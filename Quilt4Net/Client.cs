using System;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class Client
    {
        private readonly Lazy<User> _user;
        private readonly Lazy<Project> _project;
        private readonly Lazy<ISession> _session;
        private readonly Lazy<Issue> _issue;
        private readonly Lazy<IConfiguration> _configuration;

        public Client(string address = "https://www.quilt4.com/", int timeoutInSeconds = 30)
            : this(new WebApiClient(new Uri(address), new TimeSpan(0, 0, 0, timeoutInSeconds)))
        {
        }

        public Client(Uri address, TimeSpan timeout)
            : this(new WebApiClient(address, timeout))
        {
        }

        public Client(IWebApiClient webApiClient)
        {
            _configuration = new Lazy<IConfiguration>(() => new Configuration());
            _user = new Lazy<User>(() => new User(webApiClient));
            _project = new Lazy<Project>(() => new Project(webApiClient));
            _session = new Lazy<ISession>(() => new Session(webApiClient, _configuration.Value, new ApplicationHelper(_configuration.Value), new MachineHelper(), new UserHelper()));
            _issue = new Lazy<Issue>(() => new Issue(webApiClient));
        }

        public User User => _user.Value;
        public Project Project => _project.Value;
        public ISession Session => _session.Value;
        public Issue Issue => _issue.Value;
        public IConfiguration Configuration => _configuration.Value;
    }
}