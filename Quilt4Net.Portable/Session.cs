using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public abstract class Session : ISession
    {
        private readonly IWebApiClient _webApiClient;
        private readonly IConfiguration _configuration;
        private readonly IApplicationHelper _applicationHelper;
        private readonly IMachineHelper _machineHelper;
        private readonly IUserHelper _userHelper;
        private Guid _sessionKey;

        internal Session(IWebApiClient webApiClient, IConfiguration configuration, IApplicationHelper applicationHelper, IMachineHelper machineHelper, IUserHelper userHelper)
        {
            _webApiClient = webApiClient;
            _configuration = configuration;
            _applicationHelper = applicationHelper;
            _machineHelper = machineHelper;
            _userHelper = userHelper;
        }

        public bool IsRegistered => _sessionKey != Guid.Empty;

        public async Task RegisterAsync()
        {
            if (_sessionKey != Guid.Empty) throw new InvalidOperationException("The session has already been registered.");

            _sessionKey = Guid.NewGuid();

            var registerSessionRequest = new SessionData
            {
                SessionKey = _sessionKey,
                ProjectApiKey = _configuration.ProjectApiKey,
                ClientStartTime = DateTime.UtcNow,
                Environment = _configuration.Session.Environment,
                Application = _applicationHelper.GetApplicationData(),
                Machine = _machineHelper.GetMachineData(),
                User = _userHelper.GetUser(),
            };

            await _webApiClient.CreateAsync("Client/Session", registerSessionRequest);
        }

        public async Task<IEnumerable<SessionData>> GetListAsync()
        {
            throw new NotImplementedException();
        }
    }
}