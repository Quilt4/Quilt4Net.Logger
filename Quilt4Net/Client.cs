using System;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net
{
    public class Client
    {
        private readonly Lazy<User> _user;
        private readonly Lazy<Project> _project;
        private readonly Lazy<Session> _session;
        private readonly Lazy<Issue> _issue;

        public Client(IWebApiClient webApiClient)
        {
            _user = new Lazy<User>(() => new User(webApiClient));
            _project = new Lazy<Project>(() => new Project(webApiClient));
            _session = new Lazy<Session>(() => new Session(webApiClient));
            _issue = new Lazy<Issue>(() => new Issue(webApiClient));
        }

        public User User => _user.Value;
        public Project Project => _project.Value;
        public Session Session => _session.Value;
        public Issue Issue => _issue.Value;
    }
}