using System;
using System.Diagnostics;

namespace Quilt4Net.Core.DataTransfer
{
    public class SessionResponse
    {
        private readonly Stopwatch _stopWatch;
        private Exception _exception;

        public SessionResponse()
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
        public TimeSpan Elapsed => _stopWatch.Elapsed;
    }
}