using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public abstract class Session : ISession
    {
        private readonly object _syncRoot = new object();
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

        public event EventHandler<SessionRegisteredEventArgs> SessionRegisteredEvent;

        public bool IsRegistered => _sessionKey != Guid.Empty;

        public async Task<SessionResponse> RegisterAsync()
        {
            return await RegisterEx(true);
        }

        public void RegisterStart()
        {
            Task.Run(async() =>
            {
                var response = await RegisterEx(false);
                OnSessionRegisteredEvent(new SessionRegisteredEventArgs(response));
            });
        }

        public SessionResponse Register()
        {
            var result = Task<SessionResponse>.Run(async () =>
            {
                var response = await RegisterEx(false);
                OnSessionRegisteredEvent(new SessionRegisteredEventArgs(response));
                return response;
            }).Result;

            return result;
        }

        private async Task<SessionResponse>  RegisterEx(bool doThrow)
        {
            //TODO: Use a Mutex here
            //lock (_syncRoot)
            //{
                var response = new SessionResponse();

                try
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

                    //Task.Run(async () =>
                    //{
                    //    try
                    //    {
                            await _webApiClient.CreateAsync("Client/Session", registerSessionRequest);
                    //    }
                    //    catch (Exception exception)
                    //    {
                    //        Debug.WriteLine(exception.Message);
                    //        throw;
                    //    }
                    //}).Wait();
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
                    response.SetCompleted();
                }

                return response;
            //}
        }

        public async Task<IEnumerable<SessionData>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnSessionRegisteredEvent(SessionRegisteredEventArgs e)
        {
            SessionRegisteredEvent?.Invoke(this, e);
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