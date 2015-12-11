using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public class Session : ISession
    {
        private readonly IWebApiClient _webApiClient;
        private readonly IApplicationHelper _applicationHelper;
        private readonly IMachineHelper _machineHelper;
        private readonly IUserHelper _userHelper;
        private Guid _sessionKey;

        internal Session(IWebApiClient webApiClient, IApplicationHelper applicationHelper, IMachineHelper machineHelper, IUserHelper userHelper)
        {
            _webApiClient = webApiClient;
            _applicationHelper = applicationHelper;
            _machineHelper = machineHelper;
            _userHelper = userHelper;
        }

        public bool IsRegistered => _sessionKey != Guid.Empty;

        public Task RegisterAsync(string projectApiKey, string environment)
        {
            throw new NotImplementedException();
        }

        public async Task RegisterAsync(string projectApiKey, string environment, Assembly firstAssembly)
        {
            if (_sessionKey != Guid.Empty) throw new InvalidOperationException("The session has already been registered.");

            _sessionKey = Guid.NewGuid();

            var registerSessionRequest = new SessionData
            {
                SessionKey = _sessionKey,
                ProjectApiKey = projectApiKey,
                ClientStartTime = DateTime.UtcNow,
                Environment = environment,
                Application = _applicationHelper.GetApplicationData(projectApiKey, firstAssembly),
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