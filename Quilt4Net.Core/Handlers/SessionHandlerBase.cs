using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public abstract class SessionHandlerBase : ISessionHandler
    {
        private readonly object _syncRoot = new object();
        private readonly IWebApiClient _webApiClient;
        private readonly IConfiguration _configuration;
        private readonly IApplicationInformation _applicationInformation;
        private readonly IMachineInformation _machineInformation;
        private readonly IUserInformation _userInformation;
        private string _sessionToken;
        private bool _ongoingSessionRegistration;
        private bool _ongoingSessionEnding;
        private readonly AutoResetEvent _sessionRegistered = new AutoResetEvent(false);
        private readonly AutoResetEvent _sessionEnded = new AutoResetEvent(false);

        internal SessionHandlerBase(IWebApiClient webApiClient, IConfiguration configuration, IInformation information)
            : this(webApiClient, configuration, information.Aplication, information.Machine, information.User)
        {
        }

        internal SessionHandlerBase(IWebApiClient webApiClient, IConfiguration configuration, IApplicationInformation applicationInformation, IMachineInformation machineInformation, IUserInformation userInformation)
        {
            _webApiClient = webApiClient;
            _configuration = configuration;
            _applicationInformation = applicationInformation;
            _machineInformation = machineInformation;
            _userInformation = userInformation;
            ClientStartTime = DateTime.UtcNow;
        }

        public event EventHandler<SessionRegistrationStartedEventArgs> SessionRegistrationStartedEvent;
        public event EventHandler<SessionRegistrationCompletedEventArgs> SessionRegistrationCompletedEvent;
        public event EventHandler<SessionEndStartedEventArgs> SessionEndStartedEvent;
        public event EventHandler<SessionEndCompletedEventArgs> SessionEndCompletedEvent;

        public bool IsRegistered => !string.IsNullOrEmpty(_sessionToken);
        public DateTime ClientStartTime { get; }
        public string Environment => _configuration.Session != null ? _configuration.Session.Environment : string.Empty;

        public async Task<SessionResult> RegisterAsync()
        {
            return await RegisterEx(GetProjectApiKey());
        }

        public async Task<SessionResult> RegisterAsync(Assembly firstAssembly)
        {
            _applicationInformation.SetFirstAssembly(firstAssembly);
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
            _applicationInformation.SetFirstAssembly(firstAssembly);
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
            _applicationInformation.SetFirstAssembly(firstAssembly);
            return Register();
        }

        public async Task EndAsync()
        {
            if (!IsRegistered) return;
            await EndEx(await GetSessionTokenAsync());
        }

        public void End()
        {
            if (!IsRegistered) return;

            try
            {
                EndEx(GetSessionTokenAsync().Result).Wait();
            }
            catch (AggregateException exception)
            {
                throw exception.InnerException;
            }
        }

        private async Task EndEx(string sessionToken)
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

            if (string.IsNullOrEmpty(sessionToken)) throw new InvalidOperationException("There is no active session.");

            try
            {
                OnSessionEndStartedEvent(new SessionEndStartedEventArgs(sessionToken));

                await _webApiClient.ExecuteCommandAsync("Client/Session", "End", sessionToken);
            }
            catch (Exception exception)
            {
                result.SetException(exception);
                throw;
            }
            finally
            {
                _sessionToken = null;
                _ongoingSessionEnding = false;
                _sessionEnded.Set();
                result.SetCompleted();
                OnSessionEndCompletedEvent(new SessionEndCompletedEventArgs(sessionToken, result));
            }
        }

        public async Task<string> GetSessionTokenAsync()
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
                    return _sessionToken;
                }

                if (response == null)
                {
                    return _sessionToken;
                }

                return response.Response.SessionToken;
            }

            return _sessionToken;
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

        private async Task<SessionResult> RegisterEx(string projectApiKey)
        {
            if (!_configuration.Enabled)
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

            if (!string.IsNullOrEmpty(_sessionToken)) throw new SessionAlreadyRegisteredException();

            try
            {
                request = new SessionRequest
                {
                    ProjectApiKey = projectApiKey,
                    ClientStartTime = DateTime.UtcNow,
                    Environment = Environment,
                    Application = _applicationInformation.GetApplicationData(),
                    Machine = _machineInformation.GetMachineData(),
                    User = _userInformation.GetDataUser(),
                };

                OnSessionRegistrationStartedEvent(new SessionRegistrationStartedEventArgs(request));

                response = await _webApiClient.CreateAsync<SessionRequest, SessionResponse>("Client/Session", request);

                if (response.SessionToken == null) throw new InvalidOperationException("No session token returned from the server.");
                _sessionToken = response.SessionToken;
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

        //public async Task<IEnumerable<SessionResponse>> GetListAsync()
        //{
        //    //TODO: Implement
        //    throw new NotImplementedException("List sessions is not yet implemented.");
        //}

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