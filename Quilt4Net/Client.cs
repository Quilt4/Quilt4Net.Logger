using System;
using Tharga.Quilt4Net.Domain;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net
{
    public class Client
    {
        private readonly Lazy<User> _user;
        private readonly Lazy<Project> _project;

        public Client(IWebApiClient webApiClient)
        {
            _user = new Lazy<User>(() => new User(webApiClient));
            _project = new Lazy<Project>(() => new Project(webApiClient));
        }

        public User User => _user.Value;
        public Project Project => _project.Value;        
    }
}