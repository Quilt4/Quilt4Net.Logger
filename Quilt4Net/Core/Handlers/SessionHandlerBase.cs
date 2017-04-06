using System;
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
        private static int _instanceCounter;
        private string _sessionKey; // = Guid.NewGuid().ToString();
        private bool _isRegisteredOnServer;
        private string _sessionUrl;
        private bool _ongoingSessionRegistration;
        private bool _ongoingSessionEnding;
        private readonly AutoResetEvent _sessionRegistered = new AutoResetEvent(false);
        private readonly AutoResetEvent _sessionEnded = new AutoResetEvent(false);

        protected internal SessionHandlerBase(IQuilt4NetClient client)
        {
            lock (_syncRoot)
            {
                if (_instanceCounter != 0)
                {
                    if (!client.Configuration.AllowMultipleInstances)
                    {
                        throw new InvalidOperationException("Multiple instances is not allowed. Set configuration setting AllowMultipleInstances to true if you want to use multiple instances of this object.");
                    }
                }
                _instanceCounter++;
            }

            Client = client;
            ClientStartTime = DateTime.UtcNow;
        }

        public IQuilt4NetClient Client { get; }
        //public string SessionKey => _sessionKey;

        public event EventHandler<SessionRegistrationStartedEventArgs> SessionRegistrationStartedEvent;
        public event EventHandler<SessionRegistrationCompletedEventArgs> SessionRegistrationCompletedEvent;
        public event EventHandler<SessionEndStartedEventArgs> SessionEndStartedEvent;
        public event EventHandler<SessionEndCompletedEventArgs> SessionEndCompletedEvent;

        public bool IsRegisteredOnServer => _isRegisteredOnServer;
        public string SessionUrl => _sessionUrl;
        public DateTime ClientStartTime { get; }
        public string Environment => Client.Configuration.Session != null ? Client.Configuration.Session.Environment : string.Empty;
        public IApplicationInformation Application => Client.Information.Application;

        public async Task<SessionResult> RegisterAsync()
        {
            await Task.Run(() => { RegisterEx(GetProjectApiKey()); });
            throw new NotImplementedException("Wait for response from server is not yet implemented.");
        }

        public void RegisterStart()
        {
            RegisterEx(GetProjectApiKey());
            //var projectApiKey = GetProjectApiKey();

            //Task.Run(async () =>
            //    {
            //        try
            //        {
            //            await RegisterEx(projectApiKey);
            //        }
            //        catch (Exception exception)
            //        {
            //            //TODO: Just catch specific types here
            //            System.Diagnostics.Debug.WriteLine(exception.Message);
            //        }
            //    });
        }

        public SessionResult Register()
        {
            RegisterEx(GetProjectApiKey());
            throw new NotImplementedException("Wait for response from server is not yet implemented.");

            //try
            //{
            //    var response = RegisterEx(GetProjectApiKey()).Result;
            //    return response;
            //}
            //catch (AggregateException exception)
            //{
            //    throw exception?.InnerException ?? exception;
            //}
        }

        public async Task EndAsync()
        {
            if (!IsRegisteredOnServer) return;
            await EndEx(await GetSessionKeyAsync());
        }

        public void End()
        {
            if (!IsRegisteredOnServer) return;

            try
            {
                EndEx(GetSessionKeyAsync().Result).Wait();
            }
            catch (AggregateException exception)
            {
                throw exception?.InnerException ?? exception;
            }
        }

        private async Task EndEx(string sessionKey)
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

            if (string.IsNullOrEmpty(sessionKey)) throw new InvalidOperationException("There is no active session.");

            try
            {
                OnSessionEndStartedEvent(new SessionEndStartedEventArgs(sessionKey));

                throw new NotImplementedException();
                //await Client.Client.ExecuteCommandAsync("Session", "End", sessionKey);
            }
            catch (Exception exception)
            {
                result.SetException(exception);
                throw;
            }
            finally
            {
                _sessionKey = null;
                _sessionUrl = null;
                _isRegisteredOnServer = false;
                _ongoingSessionEnding = false;
                _sessionEnded.Set();
                result.SetCompleted();
                OnSessionEndCompletedEvent(new SessionEndCompletedEventArgs(sessionKey, result));
            }
        }

        public async Task<string> GetSessionKeyAsync()
        {
            if (!IsRegisteredOnServer)
            {
                RegisterEx(GetProjectApiKey());
                return _sessionKey;
            }

            return _sessionKey;
        }

        public SessionRegistrationCompletedEventArgs LastSessionRegistrationCompletedEventArgs { get; private set; }

        private string GetProjectApiKey()
        {
            var projectApiKey = Client.Configuration.ProjectApiKey;
            if (string.IsNullOrEmpty(projectApiKey))
            {
                throw new ExpectedIssues(Client.Configuration).GetException(ExpectedIssues.ProjectApiKeyNotSet);
            }
            return projectApiKey;
        }

        private void RegisterEx(string projectApiKey)
        {
            //throw new NotImplementedException();

            //    if (!Client.Configuration.Enabled)
            //    {
            //        return null;
            //    }

            //    var result = new SessionResult();
                SessionRequest request = null;
            //    SessionResponse response = null;

            //    lock (_syncRoot)
            //    {
            //        if (_ongoingSessionRegistration)
            //        {
            //            var waitTime = new TimeSpan(0, 0, 3, 0);
            //            if (!_sessionRegistered.WaitOne(waitTime))
            //            {
            //                if (string.IsNullOrEmpty(_sessionKey))
            //                {
            //                    throw new TimeoutException("Done waiting for another thread trying to get the session registered.").AddData("WaitSeconds", waitTime.TotalSeconds.ToString("0"));
            //                }
            //            }
            //        }
            //        _ongoingSessionRegistration = true;
            //    }

            //    try
            //    {
            //        if (!string.IsNullOrEmpty(_sessionKey))
            //        {
            //            result.SetAlreadyRegistered();
            //        }
            //        else
            //        {
            //            try
            //            {
            request = new SessionRequest
            {
                SessionKey = Guid.NewGuid(),
                ProjectApiKey = projectApiKey,
                ClientStartTime = DateTime.UtcNow,
                Environment = Environment,
                Application = Client.Information.Application.GetApplicationData(),
                Machine = Client.Information.Machine.GetMachineData(),
                User = Client.Information.User.GetDataUser(),
            };

            //                OnSessionRegistrationStartedEvent(new SessionRegistrationStartedEventArgs(request));

            //                response = await Client.Client.ExecuteCommandAsync<SessionResponse, SessionRequest>(request);

            //                //TODO: Put a request for a new session registration on a queue.
            //                //Have it sent and handled by the service
            //                //Wait for a response for a period of time
            //                //Return the response


            _sessionKey = Guid.NewGuid().ToString();
            Client.Client.ExecuteCommand(Guid.NewGuid(), request);

            //                //response = await Client.Client.CreateAsync<SessionRequest, SessionResponse>("Session", request);

            //                if (response?.SessionKey == null) throw new InvalidOperationException("No session key returned from the server.");
            //                _isRegisteredOnServer = true;
            //                _sessionKey = request.SessionKey.ToString(); //response.SessionKey;
            //                _sessionUrl = response.SessionUrl;
            //            }
            //            catch (Exception exception)
            //            {
            //                result.SetException(exception);
            //                throw;
            //            }
            //            finally
            //            {
            //                result.SetCompleted(response);
            //                LastSessionRegistrationCompletedEventArgs = new SessionRegistrationCompletedEventArgs(request, result);
            //                OnSessionRegistrationCompletedEvent(LastSessionRegistrationCompletedEventArgs);
            //            }
            //        }
            //    }
            //    finally
            //    {
            //        _ongoingSessionRegistration = false;
            //        _sessionRegistered.Set();
            //    }

            //    return result;
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

        public void Dispose()
        {
            End();
        }
    }
}