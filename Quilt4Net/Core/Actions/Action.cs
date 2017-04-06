using System;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.Actions
{
    public class Action : IActions
    {
        private readonly IClient _webApiClient;
        private readonly Lazy<IUser> _user;
        private readonly Lazy<IProject> _project;
        private readonly Lazy<IInvitation> _invitation;
        private readonly Lazy<IApplication> _application;
        private readonly Lazy<IVersion> _version;
        private readonly Lazy<IServerSetting> _serverSetting;
        private readonly Lazy<IService> _service;

        internal Action(IClient webApiClient)
        {
            _webApiClient = webApiClient;

            //_user = new Lazy<IUser>(() => new User(_webApiClient));
            //_project = new Lazy<IProject>(() => new Project(_webApiClient));
            //_invitation = new Lazy<IInvitation>(() => new Invitation(_webApiClient));
            //_application = new Lazy<IApplication>(() => new Application(_webApiClient));
            //_version = new Lazy<IVersion>(() => new Version(_webApiClient));
            //_serverSetting = new Lazy<IServerSetting>(() => new ServerSetting(_webApiClient));
            //_service = new Lazy<IService>(() => new Service(_webApiClient));
        }

        public IService Service => _service.Value;
        public IUser User => _user.Value;
        public IProject Project => _project.Value;
        public IInvitation Invitation => _invitation.Value;
        public IApplication Application => _application.Value;
        public IVersion Version => _version.Value;
        public IServerSetting ServerSetting => _serverSetting.Value;
    }
}