using System;
using System.Collections.Generic;
using System.Reflection;
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

        public event EventHandler<SessionRegistrationStartedEventArgs> SessionRegistrationStartedEvent;
        public event EventHandler<SessionRegistrationCompletedEventArgs> SessionRegistrationCompletedEvent;
        public event EventHandler<SessionEndStartedEventArgs> SessionEndStartedEvent;
        public event EventHandler<SessionEndCompletedEventArgs> SessionEndCompletedEvent;

        public bool IsRegistered => _sessionKey != Guid.Empty;

        public async Task<SessionResult> RegisterAsync()
        {
            return await RegisterEx(GetProjectApiKey(), true);
        }

        public async Task<SessionResult> RegisterAsync(Assembly firstAssembly)
        {
            _applicationHelper.SetFirstAssembly(firstAssembly);
            return await RegisterAsync();
        }

        public void RegisterStart()
        {
            var projectApiKey = GetProjectApiKey();

            Task.Run(async () =>
            {
                await RegisterEx(projectApiKey, false);
            });
        }

        public void RegisterStart(Assembly firstAssembly)
        {
            _applicationHelper.SetFirstAssembly(firstAssembly);
            RegisterStart();
        }

        public SessionResult Register()
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

        public SessionResult Register(Assembly firstAssembly)
        {
            _applicationHelper.SetFirstAssembly(firstAssembly);
            return Register();
        }

        public async Task EndAsync()
        {
            if (!IsRegistered) return;
            await EndEx(await GetSessionKey());
        }

        public void End()
        {
            if (!IsRegistered) return;

            try
            {
                EndEx(GetSessionKey().Result).Wait();
            }
            catch (AggregateException exception)
            {
                throw exception.InnerException;
            }
        }

        private async Task EndEx(Guid sessionKey)
        {
            await new MyMutex("Session").ExecuteAsync(async () =>
            {
                var result = new EndSesionResult();

                try
                {
                    OnSessionEndStartedEvent(new SessionEndStartedEventArgs(sessionKey));

                    await _webApiClient.ExecuteCommandAsync("Client/Session", "End", sessionKey);
                }
                catch (Exception exception)
                {
                    result.SetException(exception);
                    throw;
                }
                finally
                {
                    result.SetCompleted();
                    OnSessionEndCompletedEvent(new SessionEndCompletedEventArgs(sessionKey, result));
                    _sessionKey = Guid.Empty;
                }
            });
        }

        public async Task<Guid> GetSessionKey()
        {
            if (!IsRegistered)
            {
                var response = await RegisterEx(GetProjectApiKey(), true);
                if (_sessionKey != response.Response.SessionKey) throw new InvalidOperationException("The session key returned and stored differs.");
                return response.Response.SessionKey;
            }

            return _sessionKey;
        }

        private string GetProjectApiKey()
        {
            var projectApiKey = _configuration.ProjectApiKey;
            if (string.IsNullOrEmpty(projectApiKey))
            {
                throw new ExpectedIssues(_configuration).GetException(ExpectedIssues.ProjectApiKeyNotSet);
            }
            return projectApiKey;
        }

        private async Task<SessionResult> RegisterEx(string projectApiKey, bool doThrow)
        {
            var sessionResult = await new MyMutex("Session").ExecuteAsync(async () =>
            {
                var result = new SessionResult();
                SessionRequest request = null;

                SessionResponse response = null;

                try
                {
                    if (_sessionKey != Guid.Empty) throw new InvalidOperationException("The session has already been registered.");
                    _sessionKey = Guid.NewGuid();

                    request = new SessionRequest
                    {
                        SessionKey = _sessionKey,
                        ProjectApiKey = projectApiKey,
                        ClientStartTime = DateTime.UtcNow,
                        Environment = _configuration.Session != null ? _configuration.Session.Environment : string.Empty,
                        Application = _applicationHelper.GetApplicationData(),
                        Machine = _machineHelper.GetMachineData(),
                        User = _userHelper.GetUser(),
                    };

                    OnSessionRegistrationStartedEvent(new SessionRegistrationStartedEventArgs(request));

                    response = await _webApiClient.CreateAsync<SessionRequest, SessionResponse>("Client/Session", request);
                }
                catch (Exception exception)
                {
                    _sessionKey = Guid.Empty;
                    result.SetException(exception);

                    if (doThrow)
                        throw;
                }
                finally
                {
                    result.SetCompleted(response);
                    OnSessionRegistrationCompletedEvent(new SessionRegistrationCompletedEventArgs(request, result));
                }

                return result;

            });
            return sessionResult;
        }

        public async Task<IEnumerable<SessionRequest>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnSessionRegistrationStartedEvent(SessionRegistrationStartedEventArgs e)
        {
            SessionRegistrationStartedEvent?.Invoke(this, e);
        }

        protected virtual void OnSessionRegistrationCompletedEvent(SessionRegistrationCompletedEventArgs e)
        {
            SessionRegistrationCompletedEvent?.Invoke(this, e);
        }

        protected virtual void OnSessionEndStartedEvent(SessionEndStartedEventArgs e)
        {
            SessionEndStartedEvent?.Invoke(this, e);
        }

        protected virtual void OnSessionEndCompletedEvent(SessionEndCompletedEventArgs e)
        {
            SessionEndCompletedEvent?.Invoke(this, e);
        }
    }
}