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
        //private readonly Lazy<ISession> _session;
        //private readonly Lazy<IIssue> _issue;
        private readonly Lazy<IService> _service;

        public Action(IWebApiClient webApiClient, IConfiguration configuration)
        {
            _webApiClient = webApiClient;

            _user = new Lazy<IUser>(() => new User(_webApiClient));
            _project = new Lazy<IProject>(() => new Project(_webApiClient));
            _application = new Lazy<IApplication>(() => new Application(_webApiClient));
            _version = new Lazy<IVersion>(() => new Core.Version(_webApiClient));
            //_session = new Lazy<ISession>(() => new Core.Session(_webApiClient, configuration, new ApplicationHelper(configuration), new MachineHelper(), new UserHelper()));
            //_issue = new Lazy<IIssue>(() => new Issue(_session, _webApiClient, configuration));
            _service = new Lazy<IService>(() => new Service(_webApiClient));
        }
    }
}