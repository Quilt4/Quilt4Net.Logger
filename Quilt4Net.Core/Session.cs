using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;
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

        public event EventHandler<SessionRegisterStartedEventArgs> SessionRegisteredStartedEvent;
        public event EventHandler<SessionRegisterCompletedEventArgs> SessionRegisteredCompletedEvent;

        public bool IsRegistered => _sessionKey != Guid.Empty;

        public async Task<SessionResponse> RegisterAsync()
        {
            return await RegisterEx(GetProjectApiKey(), true);
        }

        public void RegisterStart()
        {
            var projectApiKey = GetProjectApiKey();

            Task.Run(async() =>
            {
                await RegisterEx(projectApiKey, false);
            });
        }

        public SessionResponse Register()
        {
            try
            {
                var response = RegisterEx(GetProjectApiKey(), true).Result;
                return response;
            }
            catch (AggregateException exception)
            {
                throw exception.InnerException;
            }
        }

        public Guid GetSessionKey()
        {
            if (!IsRegistered)
            {
                var response = Register();
            }

            return _sessionKey;
        }

private string GetProjectApiKey()
{
    var projectApiKey = _configuration.ProjectApiKey;
    if (string.IsNullOrEmpty(projectApiKey))
    {
        throw new ExpectedIssues.ProjectApiKeyNotSetException("?2");
    }
    return projectApiKey;
}

private async Task<SessionResponse> RegisterEx(string projectApiKey, bool doThrow)
        {
            //TODO: Use a Mutex here

            var response = new SessionResponse();
            SessionData request = null;

            try
            {
                if (_sessionKey != Guid.Empty) throw new InvalidOperationException("The session has already been registered.");
                _sessionKey = Guid.NewGuid();

                request = new SessionData
                                                 {
                                                     SessionKey = _sessionKey,
                                                     ProjectApiKey = projectApiKey,
                                                     ClientStartTime = DateTime.UtcNow,
                                                     Environment = _configuration.Session != null ? _configuration.Session.Environment : string.Empty,
                                                     Application = _applicationHelper.GetApplicationData(),
                                                     Machine = _machineHelper.GetMachineData(),
                                                     User = _userHelper.GetUser(),
                                                 };

                //TODO: Fire event here
                //OnSessionRegisteredStartedEvent(new SessionRegisterStartedEventArgs(request));

                await _webApiClient.CreateAsync("Client/Session", request);
                            //TODO: Wait for response from server here.
            }
            catch (Exception exception)
            {
                _sessionKey = Guid.Empty;
                response.SetException(exception);

                if (doThrow)
                    throw;
            }
            finally
            {
                    response.SetCompleted(_sessionKey);
                OnSessionRegisteredEvent(new SessionRegisterCompletedEventArgs(request, response));
            }

            return response;
        }

        public async Task<IEnumerable<SessionData>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnSessionRegisteredEvent(SessionRegisterCompletedEventArgs e)
        {
            SessionRegisteredCompletedEvent?.Invoke(this, e);
        }
    }
}