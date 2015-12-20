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

        private string GetProjectApiKey()
        {
            if (!IsRegistered)
            {
                var response = Register();
            }

            return _sessionKey;
        }

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

                OnSessionRegisteredStartedEvent(new SessionRegisterStartedEventArgs(request));

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
    public class IssueResponse
    {
        private readonly Stopwatch _stopWatch = new Stopwatch();
        private Exception _exception;

        public IssueResponse()
        {
            _stopWatch.Start();
        }

        public TimeSpan Elapsed => _stopWatch.Elapsed;
        public bool IsSuccess => _exception == null;
        public string ErrorMessage => _exception?.Message;

        public void SetException(Exception exception)
        {
            _exception = exception;
        }

        public void SetCompleted()
        {
            _stopWatch.Stop();
        }
    }

        private Guid _sessionKey;
        public Guid SessionKey => _sessionKey;

        public void SetCompleted(Guid sessionKey)
        {
            SessionRegisteredStartedEvent?.Invoke(this, e);
            _sessionKey = sessionKey;
        }
    }

    //internal class StopwatchHighPrecision
    //{
    //    private readonly long _frequency;
    //    private readonly long _start;
    //    private long _segment;

    //    [DllImport("Kernel32.dll")]
    //    private static extern void QueryPerformanceCounter(ref long ticks);

    //    [DllImport("Kernel32.dll")]
    //    private static extern bool QueryPerformanceFrequency(out long lpFrequency);

    //    public StopwatchHighPrecision()
    //    {
    //        QueryPerformanceFrequency(out _frequency);
    //        QueryPerformanceCounter(ref _start);
    //        _segment = _start;
    //    }

    //    public long ElapsedTotal
    //    {
    //        get
    //        {
    //            QueryPerformanceCounter(ref _segment);
    //            return (_segment - _start) * 10000000 / _frequency;
    //        }
    //    }

    //    public long ElapsedSegment
    //    {
    //        get
    //        {
    //            var last = _segment;
    //            QueryPerformanceCounter(ref _segment);
    //            return (_segment - last) * 10000000 / _frequency;
    //        }
    //    }
    //}
}