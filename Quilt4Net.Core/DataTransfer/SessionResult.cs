using System;
using System.Diagnostics;

namespace Quilt4Net.Core.DataTransfer
{
    public class EndSesionResult
    {
        private readonly Stopwatch _stopWatch;
        private Exception _exception;

        internal EndSesionResult()
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }

        public void SetException(Exception exception)
        {
            _exception = exception;
        }

        public void SetCompleted()
        {
            _stopWatch.Stop();
        }

        public bool IsSuccess => _exception == null;
        public string ErrorMessage => _exception?.Message;
        public Exception Exception => _exception;
        public TimeSpan Elapsed => _stopWatch.Elapsed;
    }

    public class SessionResult
    {
        private readonly Stopwatch _stopWatch;
        private Exception _exception;
        private SessionResponse _response;

        internal SessionResult()
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }

        public void SetException(Exception exception)
        {
            _exception = exception;
        }

        public void SetCompleted(SessionResponse response)
        {
            _response = response;
            _stopWatch.Stop();
        }

        public void SetAlreadyRegistered()
        {
            _exception = new SessionAlreadyRegisteredException();
        }

        public bool IsSuccess => _exception == null;
        public string ErrorMessage => _exception?.Message;
        public Exception Exception => _exception;
        public TimeSpan Elapsed => _stopWatch.Elapsed;
        public SessionResponse Response => _response;
    }
}