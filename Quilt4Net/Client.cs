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

        public Client(IWebApiClient webApiClient)
        {
            _user = new Lazy<User>(() => new User(webApiClient));
            _project = new Lazy<Project>(() => new Project(webApiClient));
            _session = new Lazy<ISession>(() => new Session(webApiClient, new ApplicationHelper(new Configuration()), new MachineHelper(), new UserHelper()));
            _issue = new Lazy<Issue>(() => new Issue(webApiClient));
        }

        public User User => _user.Value;
        public Project Project => _project.Value;
        public ISession Session => _session.Value;
        public Issue Issue => _issue.Value;
    }
}