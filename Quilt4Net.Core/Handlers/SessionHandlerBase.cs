using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;
using Quilt4Net.Core.Exceptions;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.Handlers
{
    public abstract class SessionHandlerBase : ISessionHandler
    {
        private readonly object _syncRoot = new object();
        private readonly IWebApiClient _webApiClient;
        private readonly IConfigurationHandler _configurationHandler;
        private readonly IApplicationLookup _applicationLookup;
        private readonly IMachineLookup _machineLookup;
        private readonly IUserLookup _userLookup;
        private Guid _sessionKey;
        private bool _ongoingSessionRegistration;
        private bool _ongoingSessionEnding;
        private readonly AutoResetEvent _sessionRegistered = new AutoResetEvent(false);
        private readonly AutoResetEvent _sessionEnded = new AutoResetEvent(false);

        internal SessionHandlerBase(IWebApiClient webApiClient, IConfigurationHandler configurationHandler, ILookup lookup)
            : this(webApiClient, configurationHandler, lookup.AplicationLookup, lookup.MachineLookup, lookup.UserLookup)
        {
        }

        internal SessionHandlerBase(IWebApiClient webApiClient, IConfigurationHandler configurationHandler, IApplicationLookup applicationLookup, IMachineLookup machineLookup, IUserLookup userLookup)
        {
            _webApiClient = webApiClient;
            _configurationHandler = configurationHandler;
            _applicationLookup = applicationLookup;
            _machineLookup = machineLookup;
            _userLookup = userLookup;
        }

        public event EventHandler<SessionRegistrationStartedEventArgs> SessionRegistrationStartedEvent;
        public event EventHandler<SessionRegistrationCompletedEventArgs> SessionRegistrationCompletedEvent;
        public event EventHandler<SessionEndStartedEventArgs> SessionEndStartedEvent;
        public event EventHandler<SessionEndCompletedEventArgs> SessionEndCompletedEvent;

        public bool IsRegistered => _sessionKey != Guid.Empty;

        public async Task<SessionResult> RegisterAsync()
        {
            return await RegisterEx(GetProjectApiKey());
        }

        public async Task<SessionResult> RegisterAsync(Assembly firstAssembly)
        {
            _applicationLookup.SetFirstAssembly(firstAssembly);
            return await RegisterAsync();
        }

        public void RegisterStart()
        {
            var projectApiKey = GetProjectApiKey();

            Task.Run(async () =>
            {
                try
                {
                    await RegisterEx(projectApiKey);
                }
                catch (Exception exception)
                {
                    //TODO: Just catch specific types here
                    System.Diagnostics.Debug.WriteLine(exception.Message);
                }
            });
        }

        public void RegisterStart(Assembly firstAssembly)
        {
            _applicationLookup.SetFirstAssembly(firstAssembly);
            RegisterStart();
        }

        public SessionResult Register()
        {
            try
            {
                var response = RegisterEx(GetProjectApiKey()).Result;
                return response;
            }
            catch (AggregateException exception)
            {
                throw exception.InnerException;
            }
        }

        public SessionResult Register(Assembly firstAssembly)
        {
            _applicationLookup.SetFirstAssembly(firstAssembly);
            return Register();
        }

        public async Task EndAsync()
        {
            if (!IsRegistered) return;
            await EndEx(await GetSessionKeyAsync());
        }

        public void End()
        {
            if (!IsRegistered) return;

            try
            {
                EndEx(GetSessionKeyAsync().Result).Wait();
            }
            catch (AggregateException exception)
            {
                throw exception.InnerException;
            }
        }

        private async Task EndEx(Guid sessionKey)
        {
            var result = new EndSesionResult();

            lock (_syncRoot)
            {
                if (_ongoingSessionEnding)
                {
                    _sessionEnded.WaitOne();
                    return;
                }

                _ongoingSessionEnding = true;
            }

            if (_sessionKey == Guid.Empty) throw new InvalidOperationException("There is no active session.");

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
                _sessionKey = Guid.Empty;
                _ongoingSessionEnding = false;
                _sessionEnded.Set();
                result.SetCompleted();
                OnSessionEndCompletedEvent(new SessionEndCompletedEventArgs(sessionKey, result));
            }
        }

        public async Task<Guid> GetSessionKeyAsync()
        {
            if (!IsRegistered)
            {
                SessionResult response = null;
                try
                {
                    response = await RegisterEx(GetProjectApiKey());
                }
                catch (SessionAlreadyRegisteredException)
                {
                    return _sessionKey;
                }

                if (response == null)
                {
                    return _sessionKey;
                }

                if (_sessionKey != response.Response.SessionKey) throw new InvalidOperationException("The session key returned and stored differs.");
                return response.Response.SessionKey;
            }

            return _sessionKey;
        }

        private string GetProjectApiKey()
        {
            var projectApiKey = _configurationHandler.ProjectApiKey;
            if (string.IsNullOrEmpty(projectApiKey))
            {
                throw new ExpectedIssues(_configurationHandler).GetException(ExpectedIssues.ProjectApiKeyNotSet);
            }
            return projectApiKey;
        }

        private async Task<SessionResult> RegisterEx(string projectApiKey)
        {
            if (!_configurationHandler.Enabled)
            {
                return null;
            }

            var result = new SessionResult();
            SessionRequest request = null;
            SessionResponse response = null;

            lock (_syncRoot)
            {
                if (_ongoingSessionRegistration)
                {
                    _sessionRegistered.WaitOne();
                    return null;
                }

                _ongoingSessionRegistration = true;
            }

            if (_sessionKey != Guid.Empty) throw new SessionAlreadyRegisteredException();

            try
            {
                var sessionKey = Guid.NewGuid();

                request = new SessionRequest
                {
                    SessionKey = sessionKey,
                    ProjectApiKey = projectApiKey,
                    ClientStartTime = DateTime.UtcNow,
                    Environment = _configurationHandler.Session != null ? _configurationHandler.Session.Environment : string.Empty,
                    Application = _applicationLookup.GetApplicationData(),
                    Machine = _machineLookup.GetMachineData(),
                    User = _userLookup.GetDataUser(),
                };

                OnSessionRegistrationStartedEvent(new SessionRegistrationStartedEventArgs(request));

                response = await _webApiClient.CreateAsync<SessionRequest, SessionResponse>("Client/Session", request);
                _sessionKey = response.SessionKey;
            }
            catch (Exception exception)
            {
                result.SetException(exception);
                throw;
            }
            finally
            {
                _ongoingSessionRegistration = false;
                _sessionRegistered.Set();
                result.SetCompleted(response);
                OnSessionRegistrationCompletedEvent(new SessionRegistrationCompletedEventArgs(request, result));
            }

            return result;
        }

        public async Task<IEnumerable<SessionResponse>> GetListAsync()
        {
            //TODO: Implement
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