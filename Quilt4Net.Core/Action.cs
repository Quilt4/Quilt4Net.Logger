using System;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public class Action : IAction
    {
        private readonly IWebApiClient _webApiClient;
        private readonly Lazy<IUser> _user;
        private readonly Lazy<IProject> _project;
        private readonly Lazy<IApplication> _application;
        private readonly Lazy<IVersion> _version;
        private readonly Lazy<IService> _service;

        public Action(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;

            _user = new Lazy<IUser>(() => new User(_webApiClient));
            _project = new Lazy<IProject>(() => new Project(_webApiClient));
            _application = new Lazy<IApplication>(() => new Application(_webApiClient));
            _version = new Lazy<IVersion>(() => new Core.Version(_webApiClient));
            _service = new Lazy<IService>(() => new Service(_webApiClient));
        }

        public IService Service => _service.Value;
        public IUser User => _user.Value;
        public IProject Project => _project.Value;
        public IApplication Application => _application.Value;
        public IVersion Version => _version.Value;
    }
}